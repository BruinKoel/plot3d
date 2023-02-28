using FireAxe.FireMath;
using FireAxe.Models.GeometryFormats;
using System.Reflection.Metadata;
using Tensorflow;

namespace FireAxe.Models
{
    public class ScalarField
    {
        private Dictionary<(int, int, int), float> field;
        private Double3m offset;
        private (Double3m, Double3m) boundingBox;

        public float tolerance { get; private set; }

        public ScalarField DeepCopy(int corner = 0)
        {
            Double3m position = 0;
            switch (corner)
            {
                case 0:
                    position = boundingBox.Item1;
                    break;
                case 1:
                    position = new Double3m(boundingBox.Item2.X, boundingBox.Item1.Y, boundingBox.Item1.Z);
                    break;
                case 2:
                    position = new Double3m(boundingBox.Item1.X, boundingBox.Item2.Y, boundingBox.Item1.Z);
                    break;
                case 3:
                    position = new Double3m(boundingBox.Item2.X, boundingBox.Item2.Y, boundingBox.Item1.Z);
                    break;
                case 4:
                    position = new Double3m(boundingBox.Item1.X, boundingBox.Item1.Y, boundingBox.Item2.Z);
                    break;
                case 5:
                    position = new Double3m(boundingBox.Item2.X, boundingBox.Item1.Y, boundingBox.Item2.Z);
                    break;
                case 6:
                    position = new Double3m(boundingBox.Item1.X, boundingBox.Item2.Y, boundingBox.Item2.Z);
                    break;
                case 7:
                    position = boundingBox.Item2;
                    break;
            }

            return new ScalarField(field.ToDictionary(entry => entry.Key, entry => entry.Value), tolerance)
            {
                boundingBox = this.boundingBox,
            };
        }

        /// <summary>
        /// Constructs a <see cref="ScalarField"/> from <paramref name="points"/> with <paramref name="tolerance"/>
        /// </summary>
        /// <param name="points"></param>
        /// <param name="tolerance"></param>
        public ScalarField(IEnumerable<Double3m> points, float tolerance)
        {
            this.boundingBox = BoundingBoxes.From(points);
            Double3m Crossbar = boundingBox.Item2 - boundingBox.Item1;
            Crossbar /= tolerance;
            Crossbar += 1;


            //field = new float[(int)Crossbar.X, (int)Crossbar.Y, (int)Crossbar.Z];

            offset = boundingBox.Item1;

            this.tolerance = tolerance;

            AddPoint(points);
        }

        /// <summary>
        /// Constructs a <see cref="ScalarField"/> from <paramref name="triangles"/> with <paramref name="tolerance"/>
        /// </summary>
        /// <param name="triangles"></param>
        /// <param name="tolerance"></param>
        public ScalarField(IEnumerable<Triangle> triangles, float tolerance)
        {
            tolerance = tolerance.Equals(-1) ? triangles.Max(x => x.v1.Z) / 200 : tolerance;

            this.boundingBox = BoundingBoxes.From(triangles);



            field = new Dictionary<(int, int, int), float>();

            field.SetDefault(new(0, 0, 0), 0);




            this.tolerance = tolerance;

            AddTriangle(triangles);

        }


        public ScalarField(Dictionary<(int, int, int), float> field, float tolerance)
        {
            this.field = field;
            this.tolerance = tolerance;
        }

        public float sum()
        {


            return field.Values.Sum();

        }
        public bool Contains(out (int, int, int) index, Double3m point)
        {
            point /= tolerance;
            index = new((int)point.X, (int)point.Y, (int)point.Z);

            if (field.ContainsKey(index))
            {

                return true;
            }
            field.Add(index, 0);
            return true;
        }
        public float GetPoint(Double3m point)
        {
            (int, int, int) index;
            if (Contains(out index, point/tolerance))
            {
                return field.GetValueOrDefault(index);
            }
            return float.NaN;
        }
        public float GetPoint((int, int, int) point)
        {
            return field.GetValueOrDefault(point);
        }
        private void AddPoint(IEnumerable<Double3m> points, float weight = 1)
        {
            (int,int,int) index;
            foreach (var point in points)
            {
                Contains(out index, point);
                field[index] = weight;
            }
        }
        public void SetPoint(Double3m point, float weight = 1)
        {
            (int, int, int) index;
            if (Contains(out index, point))
            {
                field.Remove(index);
                field.Add(index, weight);
            }

        }

        public void AddPoint(Double3m point, float weight = 1)
        {
            (int, int, int) index;
            if (Contains(out index, point))
            {
                field[index] += weight;
                return;
            }

        }


        private void AddTriangle(IEnumerable<Triangle> triangles, float weight = 1)
        {
            foreach (var triangle in triangles)
            {
                ScalarFields.TriangleFill(this, triangle, weight);
            }
        }

        private void AddTriangle(Triangle triangle, float weight = 1)
        {
            ScalarFields.TriangleFill(this, triangle, weight);

        }


        public void AddBoundingBoxIntersection((Double3m, Double3m) box)
        {
            throw new NotImplementedException();
            /*
            box.Item1 = box.Item1 - offset;
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    bool hit = false;
                    for (int z = 0; z < field.GetLength(2); z++)
                    {
                        if (field[x, y, z] > 0)
                        {
                            hit = !hit;
                            continue;
                        }
                        field[x, y, z] = hit ? 1d : 0d;
                    }
                }
            }*/
        }
        /// <summary>
        /// returns all values with their corresponding coordinates.
        /// </summary>
        public List<(Double3m, float)> values
        {
            get
            {


                return field.Keys.Select(x => (new Double3m(x), field.get(x))).ToList();
            }
        }
        /// <summary>
        /// Limits the weight of each value to a maximum of <paramref name="cap"/>
        /// </summary>
        /// <param name="cap"></param>
        public void CapWeight(float cap)
        {
            foreach (var key in field.Keys)
            {
                {
                    field[key] = field[key] > cap ? cap : field[key];

                }


            }
        }
        /// <summary>
        /// transforms all values of the field to either 1  when greater than 1 and 0 otherwise.
        /// </summary>
        public void Boolean()
        {
            foreach (var key in field.Keys)
            {
                {
                    field[key] = field[key] > 0 ? 1 : 0;

                }
            }
        }
        /// <summary>
        /// tries to fill volumes but doesn't work quite right yet
        /// </summary>
        public void RayFill()
        {
            throw new NotImplementedException();
            /*
            for (int x = 0; x < field.GetLength(0); x++)
            {
                bool lastHit = false;
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    List<int> hits = new List<int>();
                    for (int z = 0; z < field.GetLength(2); z++)
                    {
                        bool hit = field[x, y, z].Equals(0);
                        if (hit != lastHit)
                        {
                            hits.Add(z);
                        }
                        lastHit = hit;
                    }

                    for (int hit = 0; hit < hits.Count - 3; hit += 4)
                    {
                        for (int z = hits[hit + 1]; z < hits[hit + 2]; z++)
                        {

                            field[x, y, z] = 1d;
                        }
                    }
                }
            }
            */
        }

    }
}

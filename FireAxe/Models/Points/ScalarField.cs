using FireAxe.FireMath;
using FireAxe.Models.GeometryFormats;

namespace FireAxe.Models
{
    public class ScalarField
    {
        private double[,,] field;
        private Double3m offset;
        private (Double3m, Double3m) boundingBox;

        public double tolerance { get; private set; }

        public ScalarField DeepCopy(int corner = 0)
        {
            Double3m position = 0;
            switch(corner)
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

            return new ScalarField(field, tolerance) {
                boundingBox = this.boundingBox,
            };
        }

        /// <summary>
        /// Constructs a <see cref="ScalarField"/> from <paramref name="points"/> with <paramref name="tolerance"/>
        /// </summary>
        /// <param name="points"></param>
        /// <param name="tolerance"></param>
        public ScalarField(IEnumerable<Double3m> points, double tolerance)
        {
            this.boundingBox = BoundingBoxes.From(points);
            Double3m Crossbar = boundingBox.Item2 - boundingBox.Item1;
            Crossbar /= tolerance;
            Crossbar += 1;


            field = new double[(int)Crossbar.X, (int)Crossbar.Y, (int)Crossbar.Z];

            offset = boundingBox.Item1 ;

            this.tolerance = tolerance;

           AddPoint(points);
        }

        /// <summary>
        /// Constructs a <see cref="ScalarField"/> from <paramref name="triangles"/> with <paramref name="tolerance"/>
        /// </summary>
        /// <param name="triangles"></param>
        /// <param name="tolerance"></param>
        public ScalarField(IEnumerable<Triangle> triangles, double tolerance)
        {
            tolerance = tolerance.Equals(-1) ? triangles.Max(x => x.v1.Z) / 400 : tolerance;

            this.boundingBox = BoundingBoxes.From(triangles);
            Double3m Crossbar = boundingBox.Item2 - boundingBox.Item1;
            Crossbar /= tolerance;
            Crossbar += 2;


            field = new double[(int)Crossbar.X, (int)Crossbar.Y, (int)Crossbar.Z];

            offset = boundingBox.Item1 - tolerance;

            this.tolerance = tolerance;

            AddTriangle(triangles);
            
        }


        public ScalarField(double[,,] field, double tolerance)
        {
            this.field = new double[field.GetLength(0), field.GetLength(1), field.GetLength(2)];
            for(int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    for (int z = 0; z < field.GetLength(2); z++)
                    {
                        this.field[x, y, z] = field[x, y, z];
                    }
                }
            }
            this.tolerance = tolerance;
        }

        public double sum()
        {

            double sum = 0;
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    for (int k = 0; k < field.GetLength(2); k++)
                    {
                        sum += field[i, j, k];
                    }
                }
            }
            return sum;

        }
        public bool Contains(out Double3m index, Double3m point)
        {
            index = (point - offset) / tolerance;
            if (BoundingBoxes.Intersect(boundingBox, point))
            {
                return true;
            }
            return false;
        }
        public double GetPoint(Double3m point)
        {
            Double3m index;
            if (Contains(out index, point))
            {
                return field[(int)index.X, (int)index.Y, (int)index.Z];
            }
            return double.NaN;
        }
        public double GetPoint(Tuple<int, int, int> point)
        {
            if (Contains(out _, new Double3m(point.Item1, point.Item2, point.Item3)))
            {
                return field[point.Item1, point.Item2, point.Item3];
            }
            return 0;
        }
        private void AddPoint(IEnumerable<Double3m> points, double weight = 1)
        {
            foreach (var point in points)
            {
                AddPoint(point, weight);
            }
        }
        public void SetPoint(Double3m point, double weight = 1)
        {
            Double3m index;
            if (Contains(out index, point))
            {
                field[(int)index.X, (int)index.Y, (int)index.Z] = weight;
            }

        }

        public void AddPoint(Double3m point, double weight = 1)
        {
            Double3m index;
            if (Contains(out index, point))
            {
                field[(int)index.X, (int)index.Y, (int)index.Z] += weight;
                return;
            }

        }


        private void AddTriangle(IEnumerable<Triangle> triangles, double weight = 1)
        {
            foreach (var triangle in triangles)
            {
                ScalarFields.TriangleFill(this, triangle, weight);
            }
        }

        private void AddTriangle(Triangle triangle, double weight = 1)
        {
            ScalarFields.TriangleFill(this, triangle, weight);

        }


        public void AddBoundingBoxIntersection((Double3m, Double3m) box)
        {
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
            }
        }
        /// <summary>
        /// returns all values with their corresponding coordinates.
        /// </summary>
        public List<(Double3m, double)> values
        {
            get
            {

                List<(Double3m, double)> temp = new List<(Double3m, double)>();
                for (int x = 0; x < field.GetLength(0); x++)
                {
                    for (int y = 0; y < field.GetLength(1); y++)
                    {
                        for (int z = 0; z < field.GetLength(2); z++)
                        {
                            if (field[x, y, z].Equals(0)) continue;
                            temp.Add((new Double3m(x, y, z)*tolerance, field[x, y, z]));
                        }
                    }
                }
                return temp;
            }
        }
        /// <summary>
        /// Limits the weight of each value to a maximum of <paramref name="cap"/>
        /// </summary>
        /// <param name="cap"></param>
        public void CapWeight(double cap)
        {
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {

                    for (int z = 0; z < field.GetLength(2); z++)
                    {
                        if (field[x, y, z] > cap)
                        {
                            field[x, y, z] = cap;
                        }
                    }
                }
            }

        }
        /// <summary>
        /// transforms all values of the field to either 1  when greater than 1 and 0 otherwise.
        /// </summary>
        public void Boolean()
        {
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {

                    for (int z = 0; z < field.GetLength(2); z++)
                    {
                        if (field[x, y, z] > 0)
                        {
                            field[x, y, z] = 1d;
                        }
                        else
                        {
                            field[x, y, z] = 0d;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// tries to fill volumes but doesn't work quite right yet
        /// </summary>
        public void RayFill()
        {

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
        }

    }
}

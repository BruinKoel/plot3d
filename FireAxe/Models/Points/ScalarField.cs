using FireAxe.FireMath;
using FireAxe.Models.GeometryFormats;

namespace FireAxe.Models
{
    public class ScalarField
    {
        double[,,] field;
        Double3m offset;

        (Double3m, Double3m) boundingBox;

        public double tolerance { get; private set; }

        public ScalarField(IEnumerable<Double3m> points, double tolerance)
        {
            this.boundingBox = BoundingBox.From(points);
            Double3m Crossbar = boundingBox.Item2 - boundingBox.Item1;
            Crossbar /= tolerance;
            Crossbar += 2;


            field = new double[(int)Crossbar.X, (int)Crossbar.Y, (int)Crossbar.Z];

            offset = boundingBox.Item1 - tolerance;

            this.tolerance = tolerance;

            AddPoint(points);
        }
        public ScalarField(IEnumerable<Triangle> triangles, double tolerance)
        {
            this.boundingBox = BoundingBox.From(triangles);
            Double3m Crossbar = boundingBox.Item2 - boundingBox.Item1;
            Crossbar /= tolerance;
            Crossbar += 2;


            field = new double[(int)Crossbar.X, (int)Crossbar.Y, (int)Crossbar.Z];

            offset = boundingBox.Item1 - tolerance;

            this.tolerance = tolerance;

            AddTriangle(triangles);
        }

        public bool Contains(out Double3m index, Double3m point)
        {
            index = (point - offset) / tolerance;
            if (BoundingBox.Intersect(boundingBox, point))
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
        private void AddPoint(IEnumerable<Double3m> points, double weight = 1)
        {
            foreach (var point in points)
            {
                AddPoint(point, weight);
            }
        }

        public void AddPoint(Double3m point, double weight = 1)
        {
            Double3m index;
            if (Contains(out index, point))
            {
                field[(int)index.X, (int)index.Y, (int)index.Z] += weight;
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
            box.Item1 = (box.Item1 - offset);
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
                            temp.Add((new Double3m(x, y, z), field[x, y, z]));
                        }
                    }
                }
                return temp;
            }
        }

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
                            field[x, y, z] = 1;
                        }
                        else
                        {
                            field[x, y, z] = 0;
                        }
                    }
                }
            }
        }
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
                        for (int z = hits[hit+1]; z < hits[hit+2]; z++)
                        {

                            field[x, y, z] =  1d;
                        }
                    }
                }
            }
        }

    }
}

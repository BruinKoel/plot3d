using FireAxe.FireMath;

namespace FireAxe.Models
{
    public class ScalarField
    {
        double[,,] field;
        Double3m offset;
        double tolerance;

        public ScalarField(IEnumerable<Double3m> points, double tolerance)
        {
            var box = BoundingBox.From(points);
            Double3m Crossbar = box.Item2 - box.Item1;
            Crossbar /= tolerance;
            Crossbar += 2;


            field = new double[(int)Crossbar.X, (int)Crossbar.Y, (int)Crossbar.Z];

            offset = box.Item1 - tolerance;

            this.tolerance = tolerance;

            Add(points);
        }

        private void Add(IEnumerable<Double3m> points)
        {
            foreach (var point in points)
            {
                AddPoint(point);
            }
        }

        public void AddPoint(Double3m point, double weight = 1)
        {
            Double3m index = (point - offset) / tolerance;
            field[(int)index.X, (int)index.Y, (int)index.Z] += weight;
        }

        public void AddBoundingBoxIntersection((Double3m,Double3m) box)
        {
            box.Item1 = (box.Item1-offset)/ 
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

        public void RayFill()
        {
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

    }
}

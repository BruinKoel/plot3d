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


        }

        private void Fill(IEnumerable<Double3m> points)
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


    }
}

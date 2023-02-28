using FireAxe.FireMath;

namespace FireAxe.Models.Curves
{
    public class Round : Curve
    {
        public Double3m origin;
        public Double3m normal;
        public double radius;

        public Round(Double3m origin, Double3m normal, double radius)
        {
            this.origin = origin;
            this.normal = normal.Normal;
            this.radius = radius;
        }

        public override double RecommendedInterval => throw new NotImplementedException();

        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes => throw new NotImplementedException();

        public override Double3m GetPoint(double T)
        {
            T *= 2 * Math.PI;
            var point = new Double3m (Math.Sin(T), Math.Cos(T), 0d)*radius;
            return Space.Rotate(point,normal)+origin;
        }
    }
}

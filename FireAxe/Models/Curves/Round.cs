using FireAxe.FireMath;

namespace FireAxe.Models.Curves
{
    public class Round : Curve
    {
        public Double3m origin;
        public Double3m normal;
        public float radius;

        public Round(Double3m origin, Double3m normal, float radius)
        {
            this.origin = origin;
            this.normal = normal.Normal;
            this.radius = radius;
        }

        public override float RecommendedInterval => throw new NotImplementedException();

        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes => throw new NotImplementedException();

        public override Double3m GetPoint(float T)
        {
            T *= 2 * MathF.PI;
            var point = new Double3m (MathF.Sin(T), MathF.Cos(T), 0f)*radius;
            return Space.Rotate(point,normal)+origin;
        }
    }
}

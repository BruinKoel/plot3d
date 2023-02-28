namespace FireAxe.Models.Curves
{
    public class Straigth : Curve
    {
        public Double3m Direction;
        /// <summary>
        /// point to point length
        /// </summary>
        public float Length
        {
            get
            {
                return Direction.Length;
            }
        }
        public Straigth(Double3m start, Double3m end)
        {
            Direction = (end - start);
            Offset = start;

        }
        /// <inheritdoc/>
        public override ICollection<(Double3m, Double3m)> BoundingBoxes =>
            new List<(Double3m, Double3m)> { new(GetPoint(0), GetPoint(1)) };
        /// <inheritdoc/>
        public override float RecommendedInterval => 1;
        /// <inheritdoc/>
        public override Double3m GetPoint(float T)
        {
            return Direction * T + Offset;
        }
        /// <summary>
        /// Returns the Point where <paramref name="z"/> == point.z
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public Double3m InverseZ(float z)
        {
            
            float t = (z - Offset.Z) / Direction.Z;
            if (t < 0 || t > 1 || t == float.NaN) 
                return Double3m.Nan;

            return GetPoint(t);
        }
        public float MajorDimension()
        {
            float max = MathF.Max(MathF.Abs( Direction.X), MathF.Abs(Direction.Y));
            return MathF.Max(max, MathF.Abs( Direction.Z));
            
        }

    }
}

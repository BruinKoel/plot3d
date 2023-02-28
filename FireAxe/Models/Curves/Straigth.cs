namespace FireAxe.Models.Curves
{
    public class Straigth : Curve
    {
        public Double3m Direction;
        /// <summary>
        /// point to point length
        /// </summary>
        public double Length
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
        public override double RecommendedInterval => 1;
        /// <inheritdoc/>
        public override Double3m GetPoint(double T)
        {
            return Direction * T + Offset;
        }
        /// <summary>
        /// Returns the Point where <paramref name="z"/> == point.z
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public Double3m InverseZ(double z)
        {
            
            double t = (z - Offset.Z) / Direction.Z;
            if (t < 0 || t > 1 || t == double.NaN) 
                return Double3m.Nan;

            return GetPoint(t);
        }
        public double MajorDimension()
        {
            double max = Math.Max(Math.Abs( Direction.X), Math.Abs(Direction.Y));
            return Math.Max(max, Math.Abs( Direction.Z));
            
        }

    }
}

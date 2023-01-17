namespace FireAxe.Models.Curves
{
    public class Straigth : Curve
    {
        public Double3m Direction;
        
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

        public override ICollection<(Double3m, Double3m)> BoundingBoxes =>
            new List<(Double3m, Double3m)> { new(GetPoint(0), GetPoint(1)) };

        public override double RecommendedInterval => 1;

        public override Double3m GetPoint(double T)
        {
            return Direction * T + Offset;
        }

        public Double3m InverseZ(double z)
        {
            
            double t = (z - Offset.Z) / Direction.Z;
            if (t < 0 || t > 1 || t == double.NaN) 
                return Double3m.Nan;

            return GetPoint(t);
        }

    }
}

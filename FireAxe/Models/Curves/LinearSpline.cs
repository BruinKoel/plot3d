namespace FireAxe.Models.Curves
{
    public class LinearSpline : Curve
    {
        private List<Straigth> straigths;

        /// <summary>
        /// Generates a <see cref="LinearSpline"/> Going straight from point to point
        /// </summary>
        /// <param name="points"></param>
        public LinearSpline(List<Double3m> points)
        {
            Double3m prev = points.First();
            straigths = new List<Straigth>();
            for (int i = 1; i < points.Count; i++)
            {
                straigths.Add(new Straigth(prev, points[i]));
                prev = points[i];
            }
        }
        /// <summary>
        /// Generates a curde Retracing of Curves as if they are connected.
        /// </summary>
        /// <param name="curves"></param>
        public LinearSpline(List<Curve> curves) 
        {
            List<Double3m> points = new List<Double3m>();
            foreach (Curve curve in curves)
            {
                points.Add(curve.GetPoint(0));
                points.Add(curve.GetPoint(1));
            }
            points = points.Distinct(Double3m.Comparator).ToList();

            
            straigths = new List<Straigth>();
            for (int i = 1; i < points.Count; i++)
            {
                straigths.Add(new Straigth(points[i - 1], points[i]));
                
            }
        }
        /// <inheritdoc/>
        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes
        {
            get
            {
                return straigths.Select(
                    x => FireMath.BoundingBoxes.Fix((x.GetPoint(0), x.GetPoint(1))));
            }
        }
        /// <inheritdoc/>
        public override double RecommendedInterval => 1d / (double)(straigths.Count);
        /// <inheritdoc/>
        public override Double3m GetPoint(double T)
        {
            if(straigths.Count == 0) 
                return Double3m.Nan;
            if (straigths.Count == 1)
                return straigths.First().GetPoint(T);
            return straigths[(int)(T * (double)(straigths.Count - 1))]
                .GetPoint((T * (double)straigths.Count) % 1);
        }
    }
}

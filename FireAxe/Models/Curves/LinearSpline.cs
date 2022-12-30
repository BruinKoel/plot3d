namespace FireAxe.Models.Curves
{
    public class LinearSpline : Curve
    {
        private List<Straigth> straigths;
        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes
        {
            get
            {
                return straigths.Select(
                    x => FireMath.BoundingBox.Fix((x.GetPoint(0), x.GetPoint(1))));
            }
        }

        public override double RecommendedInterval => 1d / (double)(straigths.Count);

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
        public override Double3m GetPoint(double T)
        {
            if(straigths.Count == 0) 
                return null;
            if (straigths.Count == 1)
                return straigths.First().GetPoint(T);
            return straigths[(int)(T * (double)(straigths.Count - 1))]
                .GetPoint((T * (double)straigths.Count) % 1);
        }
    }
}

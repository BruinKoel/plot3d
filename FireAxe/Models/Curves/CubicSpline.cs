namespace FireAxe.Models.Curves
{
    public class CubicSpline : Curve
    {
        List<Straigth> straights;
        /// <summary>
        /// Generates a new Cubic spline with questionably good control values.
        /// </summary>
        /// <param name="points"></param>
        public CubicSpline(List<Double3m> points)
        {


            straights = new List<Straigth>();

            straights.Add(new Straigth(points.First(), points.First()));
            for (int i = 1; i < points.Count - 1; i++)
            {
                Straigth bridge = new Straigth(points[i - 1], points[i + 1]);


                straights.Add(new Straigth(points[i] - (bridge.GetPoint(0.5f) - points[i - 1])/2, points[i]));

                straights.Add(new Straigth(points[i], points[i] + (points[i + 1] - bridge.GetPoint(0.5f))/2));
            }
            straights.Add(new Straigth(points.Last(), points.Last()));
        }
        private List<(Double3m, Double3m)> boundingBoxes;

        public override float RecommendedInterval => 1f / (float)(straights.Count * 4f);
        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes
        {
            get
            {
                if (boundingBoxes != null) return boundingBoxes;

                boundingBoxes = new List<(Double3m, Double3m)>();
                Double3m previous =  GetPoint(0);
                for (float i = 1 / straights.Count; i < 1; i += 1/(float)straights.Count)
                {

                    boundingBoxes.Add(new(previous, GetPoint(i)));
                    previous = GetPoint(i);
                }

                return FireMath.BoundingBoxes.Simplify( boundingBoxes);
            }
        }

        /// <inheritdoc/>
        public override Double3m GetPoint(float T)
        {
            float t = T * ((straights.Count - 1) / 2);
            float localT = t % 1;
            int index = (int)MathF.Floor(t) * 2;

            Straigth temp = new Straigth(
                straights[index].GetPoint(localT),
                straights[index + 1].GetPoint(localT));

;

            return temp.GetPoint(localT);
        }
    }
}

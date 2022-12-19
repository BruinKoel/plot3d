namespace FireAxe.Models.Curves
{
    public class CubicSpline : Curve
    {
        List<Straigth> straights;
        public CubicSpline(List<Double3m> points)
        {


            straights = new List<Straigth>();

            straights.Add(new Straigth(points.First(), points.First()));
            for (int i = 1; i < points.Count - 1; i++)
            {
                Straigth bridge = new Straigth(points[i - 1], points[i + 1]);


                straights.Add(new Straigth(points[i] - (bridge.GetPoint(0.5) - points[i - 1])/2, points[i]));

                straights.Add(new Straigth(points[i], points[i] + (points[i + 1] - bridge.GetPoint(0.5))/2));
            }
            straights.Add(new Straigth(points.Last(), points.Last()));
        }

        public override Double3m GetPoint(double T)
        {
            double t = T * ((straights.Count - 1) / 2);
            double localT = t % 1;
            int index = (int)Math.Floor(t) * 2;

            Straigth temp = new Straigth(
                straights[index].GetPoint(localT),
                straights[index + 1].GetPoint(localT));

;

            return temp.GetPoint(localT);
        }
    }
}

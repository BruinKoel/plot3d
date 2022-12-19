namespace FireAxe.Models.Curves
{
    public class SimpleSpline : Curve2D
    {
        List<Straigth> straights;
        public SimpleSpline(List<Double3m> points)
        {


            straights = new List<Straigth>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                straights.Add(new Straigth(points[i], points[i + 1]));
            }
        }

        public override Double3m GetPoint(double T)
        {
            double t = T * (straights.Count / 2);
            double localT = t % 1;
            int index = (int)Math.Floor(t) * 2;


            Straigth temp = new Straigth(straights[index].GetPoint(localT),
                straights[index + 1].GetPoint(localT));


            return temp.GetPoint(localT);
        }

    }
}

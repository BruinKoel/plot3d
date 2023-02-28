namespace FireAxe.Models.Curves
{
    /// <summary>
    /// A Polynomial spline ^2 <see cref="Curve"/> 
    /// </summary>
    public class SimpleSpline : Curve
    {
        List<Straigth> straights;
        /// <summary>
        /// <see cref="Straigth"'s from point to point/>
        /// </summary>
        /// <param name="points"></param>
        public SimpleSpline(List<Double3m> points)
        {


            straights = new List<Straigth>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                straights.Add(new Straigth(points[i], points[i + 1]));
            }
        }
        /// <inheritdoc/>
        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes => throw new NotImplementedException();
        /// <inheritdoc/>
        public override float RecommendedInterval => 1f/(float)(straights.Count*3f);
        /// <inheritdoc/>
        public override Double3m GetPoint(float T)
        {
            float t = T * ((straights.Count - 2) / 2);
            float localT = t % 1;
            int index = (int)Math.Floor(t) * 2;


            Straigth temp = new Straigth(straights[index].GetPoint(localT),
                straights[index + 1].GetPoint(localT));


            return temp.GetPoint(localT);
        }

    }
}

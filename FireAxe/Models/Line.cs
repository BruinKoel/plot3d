using FireAxe.Models.Curves;

namespace FireAxe.Models
{
    public class Line
    {
        public Double3m Start { get { return Curve2D.GetPoint(0); } }
        public Double3m End { get { return Curve2D.GetPoint(1); } }

        private Curve Curve2D { get; set; }

        public float SegmentResolution { get; set; }

        public Line( Double3m start,  Double3m end, float depth = 1000)
        {
            Curve2D = new Straigth(start, end);
            SegmentResolution = 1 / depth;

        }



        public Double3m GetPoint(float T)
        {
            return Curve2D.GetPoint(T);
        }

        public int GetHit(int T)
        {
            int hit = 0;
            return hit;
        }


    }
}

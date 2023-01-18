namespace FireAxe.Models
{
    // <summary>
    /// Stuff that might not make the cut, but is still elligble for a comeback.
    /// </summary>
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }


        public Space Origin { get; set; }

        public Point(double x, double y, double z, Space origin)
        {
            this.X = x;
            Y = y;
            Z = z;
            Origin = origin;
        }
        public Point()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public bool Grounded
        {
            get { return Origin == null; }
        }





    }
}

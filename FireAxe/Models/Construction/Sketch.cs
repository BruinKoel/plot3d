namespace FireAxe.Models.Construction
{
    public class Sketch : Construct
    {
        private List<Curve> curves;

        public Sketch(Double3m origin, Double3m noraml)
        {
            this.origin = origin;
            this.normal = noraml;
        }
        public void AddCurve(Curve curve)
        {
            curves.Add(curve);
        }
    }
}

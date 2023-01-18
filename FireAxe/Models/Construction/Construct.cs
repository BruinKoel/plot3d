using FireAxe.Models.Construction;
using FireAxe.Models.Curves;
using FireAxe.Models.GeometryFormats;

namespace FireAxe.Models
{
    /// <summary>
    /// Container class for geometry.
    /// </summary>
    public class Construct
    {
        public Geometry geometry;

        public Construct(Geometry geometry)
        {
            this.geometry = geometry;
        }

        private double layerheight;
        List<List<Curve>> Slices;
        /// <summary>
        /// Slice object and return bag of curves
        /// </summary>
        /// <param name="layerheight"></param>
        /// <returns></returns>
        public List<List<Curve>> Slice(double layerheight = 0.4)
        {
            if (Slices != null) return Slices;

            this.layerheight = layerheight;
            Slices = new List<List<Curve>>();
            geometry.LiftZ();


            foreach (Triangle triangle in geometry.triangles)
            {
                AssignTriangle(triangle);
            }

            for (int i = 0; i < Slices.Count; i++)
            {
                if (Slices[i].Count < 2)
                {
                    Slices.RemoveAt(i);
                    i--;
                }
            }

            return Slices;
        }
        /// <summary>
        /// assign <paramref name="triangle"/> to interesctions on Z Heights.
        /// </summary>
        /// <param name="triangle"></param>
        private void AssignTriangle(Triangle triangle)
        {
            triangle.ZSort();
            while (Slices.Count <= (int)Math.Ceiling(triangle.v3.Z / layerheight) +2)
            {
                Slices.Add(new List<Curve>());
            }
            Straigth[] straigths = new Straigth[3];
            straigths[0] = new Straigth(triangle.v1, triangle.v2);
            straigths[1] = new Straigth(triangle.v2, triangle.v3);
            straigths[2] = new Straigth(triangle.v3, triangle.v1);

           

            double heightPointer = Math.Ceiling( triangle.v1.Z / layerheight)*layerheight;
            long lastcount = -1;
            while (Slices.SelectMany(x => x).Count() > lastcount)
            {
                List<Double3m> straightProjections = new List<Double3m>();
                lastcount = Slices.SelectMany(x=> x).Count();
                foreach (Straigth straigth in straigths)
                {
                    var temp = straigth.InverseZ(heightPointer);
                    if(temp != null 
                        && !temp.X.Equals(double.NaN)
                        && !temp.Y.Equals(double.NaN)
                        && !temp.Z.Equals( double.NaN) )
                        straightProjections.Add(temp);
                }
                if (straightProjections.Count > 0)
                {
                    Slices[(int)Math.Ceiling(heightPointer / layerheight)].Add(new LinearSpline(straightProjections));
                }
                
                heightPointer += layerheight;
            }

        }

    }
}

using FireAxe.Models;
using FireAxe.Models.Curves;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace FireAxe.FireMath
{
    public static class Curves
    {
        /// <summary>
        /// Simplifies a Bag of curves to a simplified list of <see cref="LinearSpline"/>
        /// </summary>
        /// <param name="slices"></param>
        /// <returns></returns>
        public static List<Curve> LinearSimplify(List<List<Curve>> slices)
        {
            int startingCount = slices.SelectMany(x => x).Count();
            List<Curve> curves = new List<Curve>();
            var result = System.Threading.Tasks.Parallel.ForEach(slices, pathes =>
            {

                var temp = getConnectedCurves(pathes);

                curves.AddRange(temp.Select(x => new LinearSpline(x)));


            });
            int finalCount = curves.Count;
            Debug.WriteLine($"removed {startingCount - finalCount} curves {(double)finalCount / (double)startingCount} ratio");
            return curves;
        }
        /// <summary>
        /// Simplifies a Bag of curves to a list of <see cref="CubicSpline"/>
        /// </summary>
        /// <param name="slices"></param>
        /// <returns></returns>
        public static List<Curve> CubicSimplify(List<List<Curve>> slices)
        {
            int startingCount = slices.SelectMany(x => x).Count();
            List<Curve> curves = new List<Curve>();
            foreach (var pathes in slices)
            {

                var temp = getConnectedCurves(pathes);

                curves.AddRange(temp.Select(x => Curves.CubicRetrace(x)));


            }
            int finalCount = curves.Count;
            Debug.WriteLine($"removed {startingCount - finalCount} curves {(double)finalCount / (double)startingCount} ratio");
            return curves;
        }
        /// <summary>
        /// Simplifies a list of curves to a <see cref="CubicSpline"/>
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static Curve CubicRetrace(List<Curve> curves)
        {
            List<Double3m> points = new();
            foreach (Curve curve in curves)
            {
                for (double T = 0; T < 1; T += curve.RecommendedInterval)
                {
                    points.Add(curve.GetPoint(T));
                }

            }
            points.Add(curves.Last().GetPoint(1));
            

            points = points.Distinct(MatrixCompare.Default).ToList();
            for (int i = 2; i < points.Count; i++)
            {


                Double3m[] segments = new Double3m[] { (points[i - 2] - points[i -1] ).Normal,
                    (points[i] - points[i -1] ).Normal
                    , Double3m.Nan };
                segments[2] = segments[1] - segments[0];

                /*if ( segments[2].LengthSquared.Equals(1d))
                {
                    points.RemoveAt(i -2);
                    
                }*/
            }
            return new CubicSpline(points);

        }
        /// <summary>
        /// Returns A bag of curves Grouped and ordered by connectedness.
        /// I think there where some currve orientations issues here? but it's been while.
        /// </summary>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static IEnumerable<List<Curve>> getConnectedCurves(List<Curve> curves)
        {
            List<List<Curve>> CurveBag = new List<List<Curve>>();
            
            while (curves.Count > 0)
            {
                List<Curve> temp = new List<Curve>();
                temp.Add(curves.First());
                
                for (int i = 0; i < curves.Count; i++)
                {
                    
                    if (IsConnected(temp.Last(), curves[i]))
                    {
                        //temp.Last().GetPoint(0d) - curves[i].GetPoint(0d);
                        if (!temp.Contains(curves[i]))
                        {
                            temp.Add(curves[i]);
                            i = -1;
                        }
                    }

                }
                CurveBag.Add(temp);
                foreach(Curve curve in temp)
                {
                    curves.Remove(curve);
                }
            }

            return CurveBag;

        }
        /// <summary>
        /// returns True if the 2 curves are connected.
        /// </summary>
        /// <param name="curve1"></param>
        /// <param name="curve2"></param>
        /// <returns></returns>
        public static bool IsConnected(Curve curve1, Curve curve2)
                => (curve1.GetPoint(0) == curve2.GetPoint(1))
                || (curve1.GetPoint(1) == curve2.GetPoint(0))
                || (curve1.GetPoint(0) == curve2.GetPoint(0))
                || (curve1.GetPoint(1) == curve2.GetPoint(1));



    }
}

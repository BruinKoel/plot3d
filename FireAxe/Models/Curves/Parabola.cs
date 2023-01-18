using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Curves
{
    /// <summary>
    /// Stuff that might not make the cut, but is still elligble for a comeback.
    /// </summary>
    public class Parabola : Curve
    {
        

        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes => throw new NotImplementedException();
        public override double RecommendedInterval => 1d / 100;
        public override Double3m GetPoint(double T)
        {
            return 0d;

        }
    }
}

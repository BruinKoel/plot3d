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
        public override float RecommendedInterval => 1f / 100f;
        public override Double3m GetPoint(float T)
        {
            return 0f;

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Curves
{
    public class Parabola : Curve
    {
        private double A;
        private double B;
        private double C;

        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes => throw new NotImplementedException();
        public override double RecommendedInterval => 1d / 100;
        public override Double3m GetPoint(double T)
        {
            return 0d;

        }
    }
}

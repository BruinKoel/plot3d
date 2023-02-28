using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Curves
{
    public class Arc : Curve
    {
        public override double RecommendedInterval => (end-start)*64;
            
        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes => throw new NotImplementedException();

        private Round baseRound;
        private double start;
        private double end;
        public Arc(Round baseRound)
        {
            this.baseRound = baseRound;
        }
        public Arc(Double3m origin, Double3m normal, double radius,double start,double end)
        {
            baseRound = new Round(origin, normal, radius);
            this.start = start;
            this.end = end;
        }

        public override Double3m GetPoint(double T)
        {
            return(baseRound.GetPoint(T * (end-start) + start));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models
{
    public abstract class Curve
    {
        public abstract Double3m GetPoint(double T);

        public abstract double RecommendedInterval { get; }
        public abstract IEnumerable<(Double3m, Double3m)> BoundingBoxes { get; }
    }
}

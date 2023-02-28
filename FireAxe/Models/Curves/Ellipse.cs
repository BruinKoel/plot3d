using FireAxe.FireMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Curves
{
    public class Ellipse : Curve
    {
        public Double3m origin;
        public Double3m normal;
        public double majorAxis;
        public double minorAxis;

        public Ellipse(Double3m origin, Double3m normal, double majorAxis, double minorAxis)
        {
            this.origin = origin;
            this.normal = normal;
            this.majorAxis = majorAxis;
            this.minorAxis = minorAxis;
        }

        public override double RecommendedInterval => throw new NotImplementedException();

        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes => throw new NotImplementedException();

        public override Double3m GetPoint(double T)
        {

            T *= 2 * Math.PI;

            var point = new Double3m(Math.Sin(T)*majorAxis, Math.Cos(T)*minorAxis, 0d);
            return Space.Rotate(point, normal) + origin;
        }
    }
}

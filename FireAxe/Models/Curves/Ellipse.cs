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
        public float majorAxis;
        public float minorAxis;

        public Ellipse(Double3m origin, Double3m normal, float majorAxis, float minorAxis)
        {
            this.origin = origin;
            this.normal = normal;
            this.majorAxis = majorAxis;
            this.minorAxis = minorAxis;
        }

        public override float RecommendedInterval => throw new NotImplementedException();

        public override IEnumerable<(Double3m, Double3m)> BoundingBoxes => throw new NotImplementedException();

        public override Double3m GetPoint(float T)
        {

            T *= 2 * MathF.PI;

            var point = new Double3m(MathF.Sin(T)*majorAxis, MathF.Cos(T)*minorAxis, 0f);
            return Space.Rotate(point, normal) + origin;
        }
    }
}

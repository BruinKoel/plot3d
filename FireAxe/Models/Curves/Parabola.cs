using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Curves
{
    public class Parabola : Curve
    {
        private double A;
        private double B;
        private double C;
        public override Point GetPoint(double T)
        {
            return new Point(T,((A * Math.Pow(T, 2)) + (B * T) + C),0,null);
        }
    }
}

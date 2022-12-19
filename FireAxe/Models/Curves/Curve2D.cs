using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Curves
{
    public abstract class Curve2D
    {
        public abstract Double3m GetPoint(double T);
    }
}

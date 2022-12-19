using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Curves
{
    public class Straigth : Curve2D
    {
        private Double3m Direction;
        private Double3m Offset;

        public Straigth(Double3m start, Double3m end)
        {
            Direction = (end - start);
            Offset = start;

        }

        public override Double3m GetPoint(double T)
        {
            return Direction*T + Offset;
        }
        

    }
}

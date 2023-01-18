using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Gym
{
    /// <summary>
    /// not used
    /// </summary>
    public class OuputPoint
    {
        public Point point;
        public OuputPoint[] supports;
        public bool grounded;
        public bool printed;

        public OuputPoint(Point point)
        {
            this.point = point;
            supports= new OuputPoint[0];
        }
    }
}

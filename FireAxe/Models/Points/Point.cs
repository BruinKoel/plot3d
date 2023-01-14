using FireAxe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models
{
    public class Point : Double3m
    {
        

        public Space Origin { get; set; }

        public Point(double x, double y, double z, Space origin = null)
        {
            this.X = x;
            Y = y;
            Z = z;
            Origin = origin;
        }
        public Point()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public bool Grounded
        {
            get { return Origin == null; }
        }

        

        

    }
}

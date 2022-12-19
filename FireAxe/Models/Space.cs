using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models
{
    public class Space
    {
        private Point Origin;
        private Space space;

        public Point GetPoint(Point point)
        {

            return (Point)(point + Origin);
        }
    }
}

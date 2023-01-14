using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models
{
    public class Space
    {
        private Double3m Origin;
        private Space space;

        public Double3m GetPoint(Double3m point)
        {

            return (point + Origin);
        }
    }
}

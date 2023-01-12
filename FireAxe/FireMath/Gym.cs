using FireAxe.Models;
using FireAxe.Models.Gym;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.FireMath
{
    public class Gym
    {
        public double CalculateFitness(IEnumerable<OuputPoint> points)
        {
            double fitness = 0;

            foreach (OuputPoint point in points)
            {
                if ( point.grounded
                    || point.supports.Any(x => x.printed))
                {
                    fitness++;
                    
                }
                else {
                    fitness--;
                }
                point.printed = true;
            }
            //if you can print all points, without any errors,the print is prefect, regardless of the path taken.
            return fitness/points.Count();
        }
    }
}

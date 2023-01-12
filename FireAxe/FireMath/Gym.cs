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
            return fitness/points.Count();
        }
    }
}

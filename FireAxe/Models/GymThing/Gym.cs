using FireAxe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Gym
{
    public class Gym
    {
        public float CalculateFitness(IEnumerable<OuputPoint> points)
        {
            float fitness = 0;

            foreach (OuputPoint point in points)
            {
                if (point.grounded
                    || point.supports.Any(x => x.printed))
                {
                    fitness++;

                }
                else
                {
                    fitness--;
                }
                point.printed = true;
            }
            //if you can print all points, without any errors,the print is prefect, regardless of the path taken.
            return fitness / points.Count();
        }
    }
}

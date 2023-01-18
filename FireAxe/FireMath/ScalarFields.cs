using FireAxe.Models;
using FireAxe.Models.Curves;
using FireAxe.Models.GeometryFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.FireMath
{
    /// <summary>
    /// Functions to do with ScalarFields
    /// </summary>
    public static class ScalarFields
    {
        /// <summary>
        /// Fills <paramref name="field"/> with <paramref name="fillWeight"/> along <paramref name="straight"/> while cell value greater or equal to <paramref name="stopThreshold"/>  
        /// </summary>
        /// <param name="field"></param>
        /// <param name="straight"></param>
        /// <param name="stopThreshold"></param>
        /// <param name="fillWeight"></param>
        public static void StraightFill(ScalarField field ,  Straigth straight, double stopThreshold = 1, double fillWeight = 1)
        {
            double stepsize = field.tolerance / straight.Length;
            for(double t = 0; t <= 1; t += stepsize)
            {
                Double3m point = straight.GetPoint(t);

                field.AddPoint(point, fillWeight);
                if ((field.GetPoint(point) - fillWeight) >= stopThreshold)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Fills a Diamond or square Area with <paramref name="fillStraight"/> along <paramref name="pathStraight"/>
        /// </summary>
        /// <param name="field"></param>
        /// <param name="pathStraight"></param>
        /// <param name="fillStraight"></param>
        /// <param name="stopThreshold"></param>
        /// <param name="fillWeight"></param>
        public static void CompoundFill(ScalarField field, Straigth pathStraight, Straigth fillStraight, double stopThreshold = 1, double fillWeight = 1)
        {
            
            double stepsize = field.tolerance / pathStraight.Length;
            for (double t = 0; t <= 1; t += stepsize)
            {
                Double3m point = pathStraight.GetPoint(t);
                fillStraight.Offset = point;
                StraightFill(field, fillStraight, stopThreshold, fillWeight);
                if ((field.GetPoint(point) - fillWeight) >= stopThreshold)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Fills <paramref name="field"/> with <paramref name="weight"/>  representing the traingle face.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="triangle"></param>
        /// <param name="weight"></param>
        public static void TriangleFill(ScalarField field, Triangle triangle, double weight = 1)
        {

            Straigth a = new Straigth(triangle.v1, triangle.v3);
            Straigth b = new Straigth(triangle.v1, triangle.v2);
            Double3m baseDirectiion = b.Direction;
            


            double stepsize = field.tolerance / a.Length;
            for (double t = 0; t <= 1; t += stepsize)
            {
                b.Offset = a.GetPoint(t);
                b.Direction = baseDirectiion * (1d-t);
                StraightFill(field, b,double.MaxValue,weight);
            }


        }
    }
}

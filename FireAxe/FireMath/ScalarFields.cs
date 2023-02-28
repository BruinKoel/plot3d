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
        public static void StraightFill(ScalarField field ,  Straigth straight, float stopThreshold = 1, float fillWeight = 1)
        {
            
            //float stepsize = (straight.MajorDimension() < field.tolerance)? straight.MajorDimension() / field.tolerance : 1f;
            float stepsize =  field.tolerance / straight.MajorDimension() ;
            if (straight.MajorDimension() < field.tolerance )
            {
                stepsize = 1;
            }

            for(float t = 0; t <= 1; t += stepsize)
            {
                Double3m point = straight.GetPoint(t);

                field.SetPoint(point, fillWeight);
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
        public static void CompoundFill(ScalarField field, Straigth pathStraight, Straigth fillStraight, float stopThreshold = 1, float fillWeight = 1)
        {
            
            float stepsize = field.tolerance / pathStraight.Length;
            stepsize /= 2;
            for (float t = 0; t <= 1; t += stepsize)
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
        public static void TriangleFill(ScalarField field, Triangle triangle, float weight = 1)
        {
            


            Straigth a = new Straigth(triangle.v1, triangle.v3);
            Straigth b = new Straigth(triangle.v2, triangle.v3);
            Double3m baseDirectiion = b.Direction;
            


            float stepsize = field.tolerance / a.MajorDimension();
            stepsize /= 2;
            for (float t = 0; t <= 1; t += stepsize)
            {
                b.Offset = a.GetPoint(t);
                b.Direction = baseDirectiion * (1f-t);
                StraightFill(field, b,float.MaxValue,weight);
            }


        }
    }
}

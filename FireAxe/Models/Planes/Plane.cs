using FireAxe.FireMath;
using FireAxe.Models.Construction;
using FireAxe.Models.Curves;
using FireAxe.Models.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Planes
{
    /// <summary>
    /// unused right now
    /// </summary>
    public class Plane : Construct
    {
        
        public Double3m origin;
        public Double3m normal;

        public Plane(Double3m origin,Double3m normal)
        {
        }
        public void Intersect(Cylinder geometry)
        {
            Sketch temp = new Sketch(origin, normal);
            

        }
        public Ellipse Project(Round round)
        {

            // Major axis direction
            Double3m majorAxisDirection = normal;

            // Minor axis direction
            Double3m minorAxisDirection = new Double3m(-normal.Y, normal.X, 0);

            // Major axis length
            float majorAxisLength = round.radius;

            // Minor axis length
            float minorAxisLength = round.radius * MathF.Sin(Space.GetAngle(this.normal, new Double3m(0, 0, 1)));

            Console.WriteLine("Ellipse parameters:");
            Console.WriteLine("Major axis direction: ({0:0.##}, {1:0.##}, {2:0.##})", majorAxisDirection.X, majorAxisDirection.Y, majorAxisDirection.Z);
            Console.WriteLine("Minor axis direction: ({0:0.##}, {1:0.##}, {2:0.##})", minorAxisDirection.X, minorAxisDirection.Y, minorAxisDirection.Z);
            Console.WriteLine(" Major axis length: {0:0.##}", majorAxisLength);
            Console.WriteLine(" Minor axis length: {0:0.##}", minorAxisLength);
            return null;
        }
    }
}

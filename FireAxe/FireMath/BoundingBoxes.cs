using FireAxe.Models;
using FireAxe.Models.Construction;
using FireAxe.Models.GeometryFormats;

namespace FireAxe.FireMath
{
    /// <summary>
    /// Class for all your boundingbox math
    /// maybe i'll make BoundingBox a type,
    /// </summary>
    public static class BoundingBoxes
    {
        /// <summary>
        /// removes unnecissary boxes from <paramref name="boxes"/>
        /// </summary>
        /// <param name="boxes"></param>
        /// <returns></returns>
        public static IEnumerable<(Double3m, Double3m)> Simplify(IEnumerable<(Double3m, Double3m)> boxes)
        {
            List<(Double3m, Double3m)> temp = new List<(Double3m, Double3m)>();
            (Double3m, Double3m) biggerbox = new();
            foreach (var box in boxes)
            {
                if (boxes.Any(x => FullyContains(x, box, out biggerbox)))
                {
                    if (!temp.Contains(biggerbox)) { temp.Add(biggerbox); }
                }
                else
                {
                    temp.Add(box);
                }

            }
            return temp;
        }

        /// <summary>
        /// Returns wether one box fully contains the other, and if so, which.
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <param name="biggestBox"></param>
        /// <returns></returns>
        public static bool FullyContains((Double3m, Double3m) box1, (Double3m, Double3m) box2, out (Double3m, Double3m) biggestBox)
        {
            box1 = Fix(box1);
            box2 = Fix(box2);

            if (box1.Item1 > box1.Item2) (box1) = (box1.Item2, box1.Item1);
            if (box2.Item1 > box2.Item2) (box2) = (box2.Item2, box2.Item1);

            if (Volume(box1) >= Volume(box2))
            {
                biggestBox = box1;
            }
            else
            {
                (box1, box2) = (box2, box1);
                biggestBox = box2;
            }

            return ((box1.Item1 <= box2.Item1) && (box1.Item2 >= box2.Item2));
        }

        /// <summary>
        /// Returns the volume of this BoundingBoxes
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static float Volume((Double3m, Double3m) box)
        {
            var temp = (box.Item2 - box.Item1);
            return temp.X * temp.Y * temp.Z;
        }

        /// <summary>
        /// generate Bounding box from
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static (Double3m, Double3m) From(Geometry geometry)
        {

            return From(geometry.vertices);
        }
        /// <summary>
        /// generate Bounding box from
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static (Double3m, Double3m) From(IEnumerable<Triangle> geometry)
        {

            return From(geometry.SelectMany(x => new []{ x.v1, x.v2, x.v3 }));
        }
        /// <summary>
        /// generate Bounding box from
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static (Double3m, Double3m) From(IEnumerable<Double3m> geometry)
        {

             (Double3m, Double3m) temp = (geometry.First(), geometry.First());
            foreach (Double3m vertex in geometry)
            {
                if (vertex.X < temp.Item1.X) temp.Item1.X = vertex.X;
                else if (vertex.X > temp.Item2.X) temp.Item2.X = vertex.X;

                if (vertex.Y < temp.Item1.Y) temp.Item1.Y = vertex.Y;
                else if (vertex.Y > temp.Item2.Y) temp.Item2.Y = vertex.Y;

                if (vertex.Z < temp.Item1.Z) temp.Item1.Z = vertex.Z;
                else if (vertex.Z > temp.Item2.Z) temp.Item2.Z = vertex.Z;
            }
            return temp;
        }
        /// <summary>
        /// return all 6 corners of the boundingbox
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static IEnumerable<Double3m> Cornered((Double3m, Double3m) box)
        {
            box = Fix(box);
            List<Double3m> temp = new List<Double3m>();
            temp.Add(box.Item1);
            temp.Add(new Double3m(box.Item1.X, box.Item1.Y, box.Item2.Z));
            temp.Add(new Double3m(box.Item1.X, box.Item2.Y, box.Item2.Z));
            temp.Add(new Double3m(box.Item1.X, box.Item2.Y, box.Item1.Z));

            temp.Add(new Double3m(box.Item2.X, box.Item2.Y, box.Item1.Z));
            temp.Add(new Double3m(box.Item2.X, box.Item1.Y, box.Item1.Z));
            temp.Add(new Double3m(box.Item2.X, box.Item1.Y, box.Item2.Z));
            temp.Add(box.Item2);

            return temp;
        }

        /// <summary>
        /// returns wether the 2 boxes intersect
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="box2"></param>
        /// <returns></returns>
        public static bool Intersect((Double3m, Double3m) box1, (Double3m, Double3m) box2)
        {
            box1 = Fix(box1);
            box2 = Fix(box2);
            return ((box1.Item1 <= box2.Item2) && (box1.Item2 >= box2.Item2));
        }
        /// <summary>
        /// returns wether the point is contained in the box
        /// intersect, contained, what even is nuance.
        /// </summary>
        /// <param name="box1"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool Intersect((Double3m, Double3m) box1, Double3m point)
        {
            box1 = Fix(box1);
            return (box1.Item1 <= point && box1.Item2 >= point);


        }
        /// <summary>
        /// makes sure that the 2 points are opposing in all dimensions.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public static (Double3m, Double3m) Fix((Double3m, Double3m) box)
        {
            if ((box.Item1 <= box.Item2))
                return box;
            if ((box.Item2 >= box.Item1))
                return (box.Item2, box.Item1);

            Double3m temp = box.Item1 - box.Item2;

            if (temp.X > 0) (box.Item1.X, box.Item2.X) = (box.Item2.X, box.Item1.X);
            if (temp.Y > 0) (box.Item1.Y, box.Item2.Y) = (box.Item2.Y, box.Item1.Y);
            if (temp.Z > 0) (box.Item1.Z, box.Item2.Z) = (box.Item2.Z, box.Item1.Z);

            return box;
        }
    }
}

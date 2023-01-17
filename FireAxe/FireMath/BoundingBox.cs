using FireAxe.Models;
using FireAxe.Models.Construction;
using FireAxe.Models.GeometryFormats;

namespace FireAxe.FireMath
{
    public static class BoundingBox
    {
        public static IEnumerable<(Double3m, Double3m)> Simplify(IEnumerable<(Double3m, Double3m)> Boxes)
        {
            List<(Double3m, Double3m)> temp = new List<(Double3m, Double3m)>();
            (Double3m, Double3m) biggerbox = new();
            foreach (var box in Boxes)
            {
                if (Boxes.Any(x => FullyContains(x, box, out biggerbox)))
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

        public static double Volume((Double3m, Double3m) box)
        {
            var temp = (box.Item2 - box.Item1);
            return temp.X * temp.Y * temp.Z;
        }

        public static (Double3m, Double3m) From(Geometry geometry)
        {

            return From(geometry.vertices);
        }

        public static (Double3m, Double3m) From(IEnumerable<Triangle> geometry)
        {

            return From(geometry.SelectMany(x => new []{ x.v1, x.v2, x.v3 }));
        }
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

        public static bool Intersect((Double3m, Double3m) box1, (Double3m, Double3m) box2)
        {
            box1 = Fix(box1);
            box2 = Fix(box2);
            return ((box1.Item1 <= box2.Item2) && (box1.Item2 >= box2.Item2));
        }
        public static bool Intersect((Double3m, Double3m) box1, Double3m point)
        {
            box1 = Fix(box1);
            return (box1.Item1 <= point && box1.Item2 >= point);


        }
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

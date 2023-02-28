using FireAxe.Models;

namespace FireAxe.FireMath
{
    public static class Space
    {
        public static Double3m Rotate(Double3m point, Double3m axis, double angle)
        {
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);

            // Rotation transformation matrix
            double[,] rotationMatrix =
            {
                {
                    cos + (axis.X * axis.X * (1 - cos)),
                    (axis.X * axis.Y * (1 - cos)) - (axis.Z * sin),
                    (axis.X * axis.Z * (1 - cos)) + (axis.Y * sin)
                },
                {
                    (axis.Y * axis.X * (1 - cos)) + (axis.Z * sin),
                    cos + (axis.Y * axis.Y * (1 - cos)),
                    (axis.Y * axis.Z * (1 - cos)) - (axis.X * sin)
                },
                {
                    (axis.Z * axis.X * (1 - cos)) - (axis.Y * sin),
                    (axis.Z * axis.Y * (1 - cos)) + (axis.X * sin),
                    cos + (axis.Z * axis.Z * (1 - cos))
                }
            };

            return new Double3m()
            {
                X = (rotationMatrix[0, 0] * point.X) + (rotationMatrix[0, 1] * point.Y) + (rotationMatrix[0, 2] * point.Z),
                Y = (rotationMatrix[1, 0] * point.X) + (rotationMatrix[1, 1] * point.Y) + (rotationMatrix[1, 2] * point.Z),
                Z = (rotationMatrix[2, 0] * point.X) + (rotationMatrix[2, 1] * point.Y) + (rotationMatrix[2, 2] * point.Z)
            };

        }

        public static Double3m Rotate(Double3m point, Double3m move)
        {
            return Rotate(
                point,
                new Double3m(0, 0, 1) ^ move,
                GetAngle(new Double3m(0, 0, 1), move));
        }

        public static double GetAngle(Double3m v1, Double3m v2)
        {
            // Angle in radians
            return Math.Acos(v1 % v2 / (v1.Length * v2.Length));

        }

        

    }
}

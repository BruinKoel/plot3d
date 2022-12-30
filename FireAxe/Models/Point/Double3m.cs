using System.Net.Security;

namespace FireAxe.Models
{
    public class Double3m
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Double3m DeepClone()
        {
            if (this == null) return new Double3m();
            return new Double3m() { X = this.X, Y = this.Y, Z = this.Z };
        }

        public double LengthSquared
        {
            get
            {
                return (X * X) + (Y * Y) + (Z * Z);
            }
        }
        public double Length
        {
            get
            {
                return Math.Sqrt(LengthSquared);
            }
        }

        public Double3m Abs
        {
            get
            {
                return new Double3m()
                {
                    X = Math.Abs(this.X),
                    Y = Math.Abs(this.Y),
                    Z = Math.Abs(this.Z)};

                }
        }

        public Double3m Normal
        {
            get
            {
                double scalair = this.Length;
                if (scalair.Equals(0)) return new Double3m();
                return this / scalair;

            }
        }
        public static MatrixCompare Comparator
        {
            get { return new MatrixCompare(); }
        }
        public static Double3m operator +(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X + matrix2.X;
            result.Y = matrix1.Y + matrix2.Y;
            result.Z = matrix1.Z + matrix2.Z;

            return result;
        }
        public static Double3m operator +(Double3m matrix1, double scalair)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X + scalair;
            result.Y = matrix1.Y + scalair;
            result.Z = matrix1.Z + scalair;

            return result;
        }
        public static Double3m operator -(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X - matrix2.X;
            result.Y = matrix1.Y - matrix2.Y;
            result.Z = matrix1.Z - matrix2.Z;

            return result;
        }
        public static Double3m operator -(Double3m matrix1, double scalair)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X - scalair;
            result.Y = matrix1.Y - scalair;
            result.Z = matrix1.Z - scalair;

            return result;
        }
        public static Double3m operator *(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X * matrix2.X;
            result.Y = matrix1.Y * matrix2.Y;
            result.Z = matrix1.Z * matrix2.Z;

            return result;
        }
        public static Double3m operator *(Double3m matrix1, double scalair)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X * scalair;
            result.Y = matrix1.Y * scalair;
            result.Z = matrix1.Z * scalair;

            return result;
        }
        public static Double3m operator /(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X / matrix2.X;
            result.Y = matrix1.Y / matrix2.Y;
            result.Z = matrix1.Z / matrix2.Z;

            return result;
        }
        public static Double3m operator /(Double3m matrix1, double scalair)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X / scalair;
            result.Y = matrix1.Y / scalair;
            result.Z = matrix1.Z / scalair;

            return result;
        }
        public static bool operator >(Double3m matrix1, Double3m matrix2)
        {
            return (matrix1.X > matrix2.X &&
                matrix1.Y > matrix2.Y &&
                matrix1.Z > matrix2.Z);
        }
        public static bool operator <(Double3m matrix1, Double3m matrix2)
        {
            return (matrix1.X < matrix2.X &&
                matrix1.Y < matrix2.Y &&
                matrix1.Z < matrix2.Z);
        }
        public static bool operator >=(Double3m matrix1, Double3m matrix2)
        {
            return (matrix1.X >= matrix2.X &&
                matrix1.Y >= matrix2.Y &&
                matrix1.Z >= matrix2.Z);
        }
        public static bool operator <=(Double3m matrix1, Double3m matrix2)
        {
            return (matrix1.X <= matrix2.X &&
                matrix1.Y <= matrix2.Y &&
                matrix1.Z <= matrix2.Z);
        }
        public static bool operator ==(Double3m matrix1, Double3m matrix2)
        {
            if(matrix1 is null && matrix2 is null) return true;
            if (matrix1 is null || matrix2 is null) return false;

            return (matrix1.X.Equals( matrix2.X) &&
                matrix1.Y.Equals(matrix2.Y) &&
                matrix1.Z.Equals(matrix2.Z));
        }
        public static bool operator !=(Double3m matrix1, Double3m matrix2)
        {
            if (matrix1 is null && matrix2 is null) return false;
            if (matrix1 is null || matrix2 is null) return true;

            return (!matrix1.X.Equals(matrix2.X) &&
                !matrix1.Y.Equals(matrix2.Y) &&
                !matrix1.Z.Equals(matrix2.Z));
        }
        public static bool operator >(Double3m matrix1, double matrix2)
        {
            return (matrix1.X > matrix2 &&
                matrix1.Y > matrix2 &&
                matrix1.Z > matrix2);
        }
        public static bool operator <(Double3m matrix1, double matrix2)
        {
            return (matrix1.X < matrix2 &&
                matrix1.Y < matrix2 &&
                matrix1.Z < matrix2);
        }
        public static bool operator >=(Double3m matrix1, double matrix2)
        {
            return (matrix1.X >= matrix2 &&
                matrix1.Y >= matrix2 &&
                matrix1.Z >= matrix2);
        }
        public static bool operator <=(Double3m matrix1, double matrix2)
        {
            return (matrix1.X <= matrix2 &&
                matrix1.Y <= matrix2 &&
                matrix1.Z <= matrix2);
        }
        public static bool operator ==(Double3m matrix1, double matrix2)
        {
            if (matrix1 == null || matrix2 == null) return false;

            return (matrix1.X.Equals(matrix2) &&
                matrix1.Y.Equals(matrix2) &&
                matrix1.Z.Equals(matrix2));
        }
        public static bool operator !=(Double3m matrix1, double matrix2)
        {
            if (matrix1 == null || matrix2 == null) return true;
            return (!matrix1.X.Equals(matrix2) &&
                !matrix1.Y.Equals(matrix2) &&
                !matrix1.Z.Equals(matrix2));
        }
    }
    public class MatrixCompare : EqualityComparer<Double3m>
    {
        private const double tolerance = 0.00001;
        public override bool Equals(Double3m matrix1, Double3m matrix2)
        {
            if (matrix1 == null && matrix2 == null)
                return true;
            else if (matrix1 == null || matrix2 == null)
                return false;

            return matrix1.X.Equals(matrix2.X)
                && matrix1.Y.Equals(matrix2.Y)
                && matrix1.Z.Equals(matrix2.Z);
        }

        public override int GetHashCode(Double3m bx)
        {
            double hCode = bx.X * bx.Y * bx.Z;
            return hCode.GetHashCode();
        }
    }
}

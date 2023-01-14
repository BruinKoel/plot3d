namespace FireAxe.Models
{
    public struct Double3m
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Double3m DeepClone()
        {
            if (this == null) return new Double3m();
            return new Double3m() { X = this.X, Y = this.Y, Z = this.Z };
        }

        public Double3m(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public static Double3m Nan
        {
            get
            {
                return new Double3m { X = double.NaN, Y = double.NaN, Z = double.NaN };
            }
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
                    Z = Math.Abs(this.Z)
                };

            }
        }
        public bool IsNan
        {
            get
            {
                return
                    (X is double.NaN
                    || Y is double.NaN
                    || Z is double.NaN);
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

        public static implicit operator Double3m(double matrix)
        {
            return new Double3m(matrix, matrix, matrix);
        }
        public static Double3m operator ^(Double3m matrix1, Double3m matrix2)
        {

            Double3m result = new Double3m();
            result.X = (matrix1.Y * matrix2.Z) - (matrix1.Z * matrix2.Y);
            result.Y = (matrix1.Z * matrix2.X) - (matrix1.X * matrix2.Z);
            result.Z = (matrix1.X * matrix2.Y) - (matrix1.Y * matrix2.X);

            return result;
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

        public static Double3m operator -(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X - matrix2.X;
            result.Y = matrix1.Y - matrix2.Y;
            result.Z = matrix1.Z - matrix2.Z;

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

        public static Double3m operator /(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X / matrix2.X;
            result.Y = matrix1.Y / matrix2.Y;
            result.Z = matrix1.Z / matrix2.Z;

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
            if (matrix1.IsNan && matrix2.IsNan) return true;
            if (matrix1.IsNan || matrix2.IsNan) return false;

            return (matrix1.X.Equals(matrix2.X) &&
                matrix1.Y.Equals(matrix2.Y) &&
                matrix1.Z.Equals(matrix2.Z));
        }
        public static bool operator !=(Double3m matrix1, Double3m matrix2)
        {
            if (matrix1 == Double3m.Nan && matrix2 == Double3m.Nan) return false;
            if (matrix1 == Double3m.Nan || matrix2 == Double3m.Nan) return true;

            return (!matrix1.X.Equals(matrix2.X) &&
                !matrix1.Y.Equals(matrix2.Y) &&
                !matrix1.Z.Equals(matrix2.Z));
        }
    }
    public class MatrixCompare : EqualityComparer<Double3m>
    {
        private const double tolerance = 0.00001;
        public override bool Equals(Double3m matrix1, Double3m matrix2)
        {
            if (matrix1.IsNan && matrix1.IsNan)
                return true;
            else if (matrix1.IsNan || matrix1.IsNan)
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

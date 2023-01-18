namespace FireAxe.Models
{
    /// <summary>
    /// 3x1 double matrix a happy life. vector3 already exists, but then i would learn nothing.
    /// </summary>
    public struct Double3m
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        
        /// <summary>
        /// Returns a deepcopy of the object, however, struct type can just return this?
        /// </summary>
        /// <returns></returns>
        public Double3m DeepClone()
        {
            return this;
            //return new Double3m() { X = this.X, Y = this.Y, Z = this.Z };
        }
        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public Double3m(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        /// <summary>
        /// coordiante doubles are double.Nan
        /// </summary>
        public static Double3m Nan
        {
            get
            {
                return new Double3m { X = double.NaN, Y = double.NaN, Z = double.NaN };
            }
        }
        /// <summary>
        /// returns the squared length when enterpenting as a vector.
        /// </summary>
        public double LengthSquared
        {
            get
            {
                return (X * X) + (Y * Y) + (Z * Z);
            }
        }
        /// <summary>
        /// returns the length when enterpenting as a vector.
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(LengthSquared);
            }
        }
        /// <summary>
        /// represents the Absolute Coordinates.
        /// </summary>
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
        /// <summary>
        /// Returns a rounded down version of the coordinates
        /// </summary>
        public Double3m Rounded
        {
            get
            {
                return new Double3m()
                {
                    X = Math.Round(this.X),
                    Y = Math.Round(this.Y),
                    Z = Math.Round(this.Z)
                };
            }
            
        }
        /// <summary>
        /// is true when any of the coordinate components are Nan.
        /// </summary>
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

        /// <summary>
        /// returns a normalised representation of the matrix.
        /// </summary>
        public Double3m Normal
        {
            get
            {
                double scalair = this.Length;
                if (scalair.Equals(0)) return new Double3m();
                return this / scalair;

            }
        }


        /// <summary>
        /// converts a single double to a matrix filled with
        /// </summary>
        /// <param name="matrix"></param>
        public static implicit operator Double3m(double matrix)
        {
            return new Double3m(matrix, matrix, matrix);
        }
        /// <summary>
        /// Cross product
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Double3m operator ^(Double3m matrix1, Double3m matrix2)
        {

            Double3m result = new Double3m();
            result.X = (matrix1.Y * matrix2.Z) - (matrix1.Z * matrix2.Y);
            result.Y = (matrix1.Z * matrix2.X) - (matrix1.X * matrix2.Z);
            result.Z = (matrix1.X * matrix2.Y) - (matrix1.Y * matrix2.X);

            return result;
        }
        /// <summary>
        /// lazy boi
        /// </summary>
        public static MatrixCompare Comparator
        {
            get { return new MatrixCompare(); }
        }
        /// <summary>
        /// +
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Double3m operator +(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X + matrix2.X;
            result.Y = matrix1.Y + matrix2.Y;
            result.Z = matrix1.Z + matrix2.Z;

            return result;
        }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Double3m operator -(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X - matrix2.X;
            result.Y = matrix1.Y - matrix2.Y;
            result.Z = matrix1.Z - matrix2.Z;

            return result;
        }
        /// <summary>
        /// *
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Double3m operator *(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X * matrix2.X;
            result.Y = matrix1.Y * matrix2.Y;
            result.Z = matrix1.Z * matrix2.Z;

            return result;
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static Double3m operator /(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1.DeepClone();
            result.X = matrix1.X / matrix2.X;
            result.Y = matrix1.Y / matrix2.Y;
            result.Z = matrix1.Z / matrix2.Z;

            return result;
        }
        /// <summary>
        /// Greater than
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator >(Double3m matrix1, Double3m matrix2)
        {
            return (matrix1.X > matrix2.X &&
                matrix1.Y > matrix2.Y &&
                matrix1.Z > matrix2.Z);
        }
        /// <summary>
        /// Lesser than
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator <(Double3m matrix1, Double3m matrix2)
        {
            return (matrix1.X < matrix2.X &&
                matrix1.Y < matrix2.Y &&
                matrix1.Z < matrix2.Z);
        }
        /// <summary>
        /// Equal or greater than
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator >=(Double3m matrix1, Double3m matrix2)
        {
            return (matrix1.X >= matrix2.X &&
                matrix1.Y >= matrix2.Y &&
                matrix1.Z >= matrix2.Z);
        }
        /// <summary>
        /// Equal or greater than
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator <=(Double3m matrix1, Double3m matrix2)
        {
            return (matrix1.X <= matrix2.X &&
                matrix1.Y <= matrix2.Y &&
                matrix1.Z <= matrix2.Z);
        }
        /// <summary>
        /// equals
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator ==(Double3m matrix1, Double3m matrix2)
        {
            if (matrix1.IsNan && matrix2.IsNan) return true;
            if (matrix1.IsNan || matrix2.IsNan) return false;

            return (matrix1.X.Equals(matrix2.X) &&
                matrix1.Y.Equals(matrix2.Y) &&
                matrix1.Z.Equals(matrix2.Z));
        }
        /// <summary>
        /// is not equal to
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator !=(Double3m matrix1, Double3m matrix2)
        {
            if (matrix1 == Double3m.Nan && matrix2 == Double3m.Nan) return false;
            if (matrix1 == Double3m.Nan || matrix2 == Double3m.Nan) return true;

            return (!matrix1.X.Equals(matrix2.X) &&
                !matrix1.Y.Equals(matrix2.Y) &&
                !matrix1.Z.Equals(matrix2.Z));
        }
    }
    /// <summary>
    /// EqualityComparer because of some linq thing that didn't allow some lambda
    /// </summary>
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
            double hCode = bx.X * bx.Y * bx.Z;// not unique i know, this is just a quick kinda accurate dirty fix
            return hCode.GetHashCode();
        }
    }
}

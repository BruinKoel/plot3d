using System.Collections;

namespace FireAxe.Models
{
    /// <summary>
    /// 3x1 float matrix a happy life. vector3 already exists, but then i would learn nothing.
    /// </summary>
    public struct Double3m
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

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
        public Double3m(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Double3m((int,int,int) pos)
        {
            this.X = pos.Item1;
            this.Y = pos.Item2;
            this.Z = pos.Item3;
        }

        public Double3m(Tuple<float, float, float> point)
        {
            this.X = point.Item1;
            this.Y = point.Item2;
            this.Z = point.Item3;
        }

        public Double3m(Tuple<int, int, int> point)
        {
            this.X = point.Item1;
            this.Y = point.Item2;
            this.Z = point.Item3;
        }

        /// <summary>
        /// coordiante floats are float.Nan
        /// </summary>
        public static Double3m Nan
        {
            get
            {
                return new Double3m { X = float.NaN, Y = float.NaN, Z = float.NaN };
            }
        }
        /// <summary>
        /// returns the squared length when enterpenting as a vector.
        /// </summary>
        public float LengthSquared
        {
            get
            {
                return (X * X) + (Y * Y) + (Z * Z);
            }
        }
        /// <summary>
        /// returns the length when enterpenting as a vector.
        /// </summary>
        public float Length
        {
            get
            {
                return MathF.Sqrt(LengthSquared);
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
                    X = MathF.Abs(this.X),
                    Y = MathF.Abs(this.Y),
                    Z = MathF.Abs(this.Z)
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
                    X = MathF.Round(this.X),
                    Y = MathF.Round(this.Y),
                    Z = MathF.Round(this.Z)
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
                    X is float.NaN
                    || Y is float.NaN
                    || Z is float.NaN;
            }
        }

        /// <summary>
        /// returns a normalised representation of the matrix.
        /// </summary>
        public Double3m Normal
        {
            get
            {
                float scalair = this.Length;
                if (scalair.Equals(0)) return new Double3m();
                return this / scalair;

            }
        }


        /// <summary>
        /// converts a single float to a matrix filled with
        /// </summary>
        /// <param name="matrix"></param>
        public static implicit operator Double3m(float matrix)
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
        /// Dot product
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static float operator %(Double3m matrix1, Double3m matrix2)
        {
            Double3m result = matrix1 * matrix2;
            return result.X + result.Y + result.Z;
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
            Double3m result = new Double3m();
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
            Double3m result = new Double3m();
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
            Double3m result = new Double3m();
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
            Double3m result = new Double3m();
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
            return matrix1.X > matrix2.X &&
                matrix1.Y > matrix2.Y &&
                matrix1.Z > matrix2.Z;
        }
        /// <summary>
        /// Greater than
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator >>(Double3m matrix1, Double3m matrix2)
        {
            return matrix1.X > matrix2.X ||
                matrix1.Y > matrix2.Y ||
                matrix1.Z > matrix2.Z;
        }
        /// <summary>
        /// Lesser than
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator <(Double3m matrix1, Double3m matrix2)
        {
            return matrix1.X < matrix2.X &&
                matrix1.Y < matrix2.Y &&
                matrix1.Z < matrix2.Z;
        }
        /// <summary>
        /// Lesser than
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator <<(Double3m matrix1, Double3m matrix2)
        {
            return matrix1.X < matrix2.X ||
                matrix1.Y < matrix2.Y ||
                matrix1.Z < matrix2.Z;
        }
        /// <summary>
        /// Equal or greater than
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator >=(Double3m matrix1, Double3m matrix2)
        {
            return matrix1.X >= matrix2.X &&
                matrix1.Y >= matrix2.Y &&
                matrix1.Z >= matrix2.Z;
        }
        /// <summary>
        /// Equal or greater than
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        public static bool operator <=(Double3m matrix1, Double3m matrix2)
        {
            return matrix1.X <= matrix2.X &&
                matrix1.Y <= matrix2.Y &&
                matrix1.Z <= matrix2.Z;
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

            return matrix1.X.Equals(matrix2.X) &&
                matrix1.Y.Equals(matrix2.Y) &&
                matrix1.Z.Equals(matrix2.Z);
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

            return !matrix1.X.Equals(matrix2.X) ||
                !matrix1.Y.Equals(matrix2.Y) ||
                !matrix1.Z.Equals(matrix2.Z);
        }
    }
    /// <summary>
    /// EqualityComparer because of some linq thing that didn't allow some lambda
    /// </summary>
    public class MatrixCompare : EqualityComparer<Double3m>
    {
        private const float tolerance = 0.00001f;
        public override bool Equals(Double3m matrix1, Double3m matrix2)
        {
            return matrix1 == matrix2;
        }

        public override int GetHashCode(Double3m bx)
        {
            // :D
            var bitArray = new BitArray(BitConverter.GetBytes(bx.X));
            bitArray.Xor(new BitArray(BitConverter.GetBytes(bx.Y)));
            bitArray.Xor(new BitArray(BitConverter.GetBytes(bx.Z)));

            return bitArray.GetHashCode();
        }
    }
}

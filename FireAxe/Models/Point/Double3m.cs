using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            return new Double3m() { X = this.X, Y = this.Y, Z = this.Z};
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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Importer.Models
{
    public class STL
    {
        public byte[] header;
        public List<Triangle> triangles;
        public List<int> indices;
        public Dictionary<int, Point3D> vertices;
        public STL(byte[] data)
        {
            indices = new List<int>();
            header = data[..80];
            int triangleP = 84;
            int expectedTriangles = data[BitConverter.ToInt32(data[80..84])];//bro, wtf
            triangles = new List<Triangle>();

            for (int i = 0; i < (data.Length-84)/50; i++)
            {
                triangles.Add(readTriangleBytes(data[triangleP..(triangleP += 50)]));
            }

        }
        public Triangle readTriangleBytes(byte[] data)
        {
            int pointer = 0;
                Triangle triangle = new Triangle();
            if (!BitConverter.IsLittleEndian)
            {
                for(int i = 0; i < data.Length; i += 4)
                {
                    (data[i], data[i+1]) = (data[i+2], data[i+3]);
                }
            }

            triangle.normal = new Vector3D(
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]));

            triangle.v1 = new Point3D(
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]));

            triangle.v2 = new Point3D(
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]));

            triangle.v3 = new Point3D(
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]));




            triangle.count = BitConverter.ToUInt16(data[pointer..(pointer += 2)]);
            return triangle;
        }

        public double getMaxDistance(Point3D p1, Point3D p2)
        {
            double tempo = Math.Abs(p1.X - p2.X);
            tempo += Math.Abs(p1.Y - p2.Y);
            return tempo + Math.Abs(p1.Z - p2.Z);
        }
        public void CalculateIndices(double modeltolerance = 0.00000001)
        {
            int count = 0;
            vertices = new Dictionary<int, Point3D>();

            foreach (Triangle triangle in this.triangles)
            {
                
                foreach (Point3D vertex in new[] { triangle.v1, triangle.v2, triangle.v3 })
                {


                    IEnumerable<KeyValuePair<int, Point3D>> point = vertices.Where(x => getMaxDistance(x.Value, vertex) < modeltolerance);

                    if (point.Count() != 0)
                    {
                        indices.Add(point.First().Key);
                    }
                    else
                    {
                        indices.Add(count);
                        vertices.Add(count++, vertex);
                    }
                }
            }
        }
        //Debug.WriteLine(" ");

    }


}
public class Triangle
{
    public Vector3D normal;
    public Point3D v1;
    public Point3D v2;
    public Point3D v3;
    public UInt16 count;
    public Triangle()
    {
        normal = new Vector3D();
        v1 = new Point3D();
        v2 = new Point3D();
        v3 = new Point3D();
        //VSort();
    }
    public void VSort()
    {
        double p1 = 0;
        double p2 = 0;
        double p3 = 0;

        if (p3 > p2 || p2 > p1)
        {
            return;
        }
        if (p2 < p3)
        {
            if (p1 > p3)
                (this.v1, this.v2, this.v3) = (this.v2, this.v3, this.v1);
            else
            {
                (this.v1, this.v2) = (this.v2, this.v1);
            }
            return;
        }
        else
        {
            if (p1 > p2)
            {
                (this.v1, this.v2, this.v3) = (this.v3, this.v2, this.v1);
            }
            else
            {
                (this.v1, this.v3) = (this.v3, this.v1);
            }
            return;
        }
        Console.WriteLine("fuck");
    }


}



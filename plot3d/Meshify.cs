using FireAxe.Models;
using FireAxe.Models.Curves;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace plot3d
{
    public static class Meshify
    {
        public static MeshGeometry3D Mesh(object kek)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            return mesh;
        }

        public static MeshGeometry3D Mesh(Curve2D line, double width = 0.02)
        {
            double SegmentResolution = 1d / 512d;
            if (line.GetType() == typeof(Straigth)) SegmentResolution = 1;


            width /= 2;
            List<Double3m> segments = new List<Double3m>();
            MeshGeometry3D mesh = new MeshGeometry3D();
            for(double T = 0; T <= 1; T += SegmentResolution)
            {
                
                segments.Add( line.GetPoint(T));
            }
            var segmentEnumerator = segments.GetEnumerator();
            segmentEnumerator.MoveNext();
            Double3m previousPoint = segmentEnumerator.Current;
            int index = 0;
            while (segmentEnumerator.MoveNext())
            {
                mesh.Positions.Add(As3D((previousPoint-width)));
                mesh.Positions.Add(As3D((previousPoint + width)));
                mesh.Positions.Add(As3D((segmentEnumerator.Current)));
                mesh.TriangleIndices.Add(index++);
                mesh.TriangleIndices.Add(index++);
                mesh.TriangleIndices.Add(index++);

                mesh.Positions.Add(As3D((segmentEnumerator.Current - width)));
                mesh.Positions.Add(As3D((segmentEnumerator.Current + width)));
                mesh.Positions.Add(As3D((previousPoint)));
                mesh.TriangleIndices.Add(index++);
                mesh.TriangleIndices.Add(index++);
                mesh.TriangleIndices.Add(index++);
                Debug.WriteLine(Math.Pow(previousPoint.X, 2) + Math.Pow(previousPoint.Y ,2));
                previousPoint = segmentEnumerator.Current;
            }
                

            return mesh;
        }

        public static void addTriangle(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3)
        {
            int index1 = addPoint(mesh.Positions, p1);
            int index2 = addPoint(mesh.Positions, p2);
            int index3 = addPoint(mesh.Positions, p3);
            mesh.TriangleIndices.Add(index1);
            mesh.TriangleIndices.Add(index2);
            mesh.TriangleIndices.Add(index3);
        }
        private static int addPoint(Point3DCollection positions, Point3D point)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if ((point.X == positions[i].X) &&
                    (point.Y == positions[i].Y) &&
                    (point.Z == positions[i].Z))
                    return i;
            }
            positions.Add(point);
            return positions.Count - 1;
        }
        public static Point3D As3D(Double3m point) => new Point3D(point.X, point.Y, point.Z);
    }
}

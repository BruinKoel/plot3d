using FireAxe.Models;
using FireAxe.Models.Construction;
using FireAxe.Models.Curves;
using FireAxe.Models.GeometryFormats;
using System.Collections.Generic;
using System.Linq;
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

        public static MeshGeometry3D MeshWireframe(Geometry geometry)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            foreach (Triangle triangle in geometry.triangles)
            {
                mesh = MeshCurve(
                    mesh,
                    new LinearSpline(
                        new List<Double3m>()
                        {
                            triangle.v1,
                            triangle.v2,
                            triangle.v3,
                            triangle.v1
                        }));
            }
            return mesh;
        }
        public static MeshGeometry3D MeshWireframeBoundingBoxes(Geometry geometry)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            foreach (Triangle triangle in geometry.triangles)
            {
                mesh = MeshBoundingBoxes(
                    mesh,
                    new LinearSpline(
                        new List<Double3m>()
                        {
                            triangle.v1,
                            triangle.v2,
                            triangle.v3,
                            triangle.v1})
                    );
            }
            return mesh;
        }
        public static MeshGeometry3D MeshCurve(Curve line, double width = 0d)
        {
            return MeshCurve(new MeshGeometry3D(), line, width);
        }

        public static MeshGeometry3D MeshCurve(IEnumerable<Curve> line, double width = 0d)
        {
            _ = line.Count();
            var mesh = new MeshGeometry3D();
            foreach (Curve curve in line)
            {
                if (curve == null || curve.GetPoint(0) == null) 
                    continue;
                mesh = MeshCurve(mesh, curve, width);
            }
            return mesh;
        }

        public static MeshGeometry3D MeshCurve(MeshGeometry3D mesh, Curve line, double width = 0d)
        {
            double SegmentResolution = line.RecommendedInterval;
            if (width.Equals(0d))
            {
                width = (line.GetPoint(0) - line.GetPoint(1)).Length * 0.02;
                if (width > 0.01) width = 0.1;
                if (width < 0.004) width = 0.001;
            }
            width /= 2;
            List<Double3m> segments = new List<Double3m>();
            mesh ??= new MeshGeometry3D();
            for (double T = 0; T <= 1; T += SegmentResolution)
            {

                segments.Add(line.GetPoint(T));
            }
            var segmentEnumerator = segments.GetEnumerator();
            segmentEnumerator.MoveNext();
            Double3m previousPoint = segmentEnumerator.Current;
            int index = mesh.Positions.Count;
            while (segmentEnumerator.MoveNext())
            {
                mesh.Positions.Add(As3D((previousPoint - width)));
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
                //Debug.WriteLine(Math.Pow(previousPoint.X, 2) + Math.Pow(previousPoint.Y ,2));
                previousPoint = segmentEnumerator.Current;
            }


            return mesh;
        }
        public static MeshGeometry3D MeshBoundingBoxes(MeshGeometry3D mesh, Curve line)
        {
            mesh ??= new MeshGeometry3D();
            foreach (var box in line.BoundingBoxes)
            {
                mesh = MeshBox(mesh, box);
            }
            return mesh;
        }

        public static MeshGeometry3D MeshBox(MeshGeometry3D mesh, (Double3m, Double3m) box)
        {
            int index = mesh.Positions.Count;

            foreach (Double3m v in FireAxe.FireMath.BoundingBox.Expanded(box))
            {
                mesh.Positions.Add(As3D(v));
            }

            mesh.TriangleIndices.Add(index + 0);
            mesh.TriangleIndices.Add(index + 1);
            mesh.TriangleIndices.Add(index + 2);

            mesh.TriangleIndices.Add(index + 2);
            mesh.TriangleIndices.Add(index + 3);
            mesh.TriangleIndices.Add(index + 0);

            mesh.TriangleIndices.Add(index + 0);
            mesh.TriangleIndices.Add(index + 5);
            mesh.TriangleIndices.Add(index + 6);

            mesh.TriangleIndices.Add(index + 6);
            mesh.TriangleIndices.Add(index + 1);
            mesh.TriangleIndices.Add(index + 0);

            mesh.TriangleIndices.Add(index + 0);
            mesh.TriangleIndices.Add(index + 3);
            mesh.TriangleIndices.Add(index + 4);

            mesh.TriangleIndices.Add(index + 4);
            mesh.TriangleIndices.Add(index + 5);
            mesh.TriangleIndices.Add(index + 0);

            mesh.TriangleIndices.Add(index + 7);
            mesh.TriangleIndices.Add(index + 6);
            mesh.TriangleIndices.Add(index + 5);

            mesh.TriangleIndices.Add(index + 5);
            mesh.TriangleIndices.Add(index + 4);
            mesh.TriangleIndices.Add(index + 7);

            mesh.TriangleIndices.Add(index + 7);
            mesh.TriangleIndices.Add(index + 2);
            mesh.TriangleIndices.Add(index + 1);

            mesh.TriangleIndices.Add(index + 1);
            mesh.TriangleIndices.Add(index + 6);
            mesh.TriangleIndices.Add(index + 7);

            mesh.TriangleIndices.Add(index + 7);
            mesh.TriangleIndices.Add(index + 4);
            mesh.TriangleIndices.Add(index + 3);

            mesh.TriangleIndices.Add(index + 3);
            mesh.TriangleIndices.Add(index + 2);
            mesh.TriangleIndices.Add(index + 7);

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

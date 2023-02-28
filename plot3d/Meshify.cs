using FireAxe.Models;
using FireAxe.Models.Construction;
using FireAxe.Models.Curves;
using FireAxe.Models.GeometryFormats;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace plot3d
{
    /// <summary>
    /// This class just converts from My types to representative Geometry3D models. 
    /// </summary>
    public static class Meshify
    {
        /// <summary>
        /// does nothing yet,but maybe a single function to convert other edge cases? probably delete soon
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static MeshGeometry3D Mesh(object obj)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            return mesh;
        }
        /// <summary>
        /// does nothing yet,but maybe a single function to convert other edge cases? probably delete soon
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static MeshGeometry3D Mesh(Geometry geometry)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.TriangleIndices = new System.Windows.Media.Int32Collection(geometry.indices);
            mesh.Positions = new Point3DCollection(geometry.vertices.Select(x => As3D(x)));

            return mesh;
        }
        /// <summary>
        /// Produces a Mesh of floating triangles representing each value in the field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static MeshGeometry3D MeshScalarField(ScalarField field)
        {
            field.CapWeight(4f);
            List<Triangle> triangles = new List<Triangle>();
            foreach (var value in field.values)
            {
                triangles.Add(TriangleFromPoint(value.Item1, value.Item2 / 2));
            }
            Geometry geometry = new Form(triangles, true);

            MeshGeometry3D mesh = new MeshGeometry3D();




            mesh.TriangleIndices = new System.Windows.Media.Int32Collection(geometry.indices);
            mesh.Positions = new Point3DCollection(geometry.vertices.Select(x => Meshify.As3D(x)));

            return mesh;

        }
        /// <summary>
        /// generates a traingle at <paramref name="point"/> of size <paramref name="scale"/>
        /// </summary>
        /// <param name="point"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        private static Triangle TriangleFromPoint(Double3m point, float scale)
        {
            Triangle triangle = new Triangle(
                point,
                point + scale,
                point + new Double3m(-scale, scale, scale));
            return triangle;
        }
        /// <summary>
        /// generates a Wireframe geometry 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
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
        /// <summary>
        ///  mesh a boundingbox for every traingle or something?
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
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
        /// <summary>
        /// generate a mesh from <paramref name="line"/> with <paramref name="width"/>
        /// </summary>
        /// <param name="line"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static MeshGeometry3D MeshCurve(Curve line, float width = 0f)
        {
            return MeshCurve(new MeshGeometry3D(), line, width);
        }

        /// <summary>
        /// generate a mesh from all <paramref name="line"/>'s with <paramref name="width"/>
        /// </summary>
        /// <param name="line"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static MeshGeometry3D MeshCurve(IEnumerable<Curve> line, float width = 0f)
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

        public static MeshGeometry3D MeshMoves(List<Double3m> moves)
        {
            Double3m currentpos = 0;
            List<Double3m> points = new List<Double3m>();
            points.Add(currentpos);

            MeshGeometry3D mesh = new MeshGeometry3D();
            foreach (Double3m move in moves)
            {
                currentpos += move;
                points.Add(currentpos);

            }
            return MeshCurve(new LinearSpline(points),0.05f);

        }

        /// <summary>
        /// Generate a mesh from <paramref name="line"/> and add it to <paramref name="mesh"/>
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="line"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static MeshGeometry3D MeshCurve(MeshGeometry3D mesh, Curve line, float width = 0f)
        {
            float SegmentResolution = line.RecommendedInterval;
            if (width.Equals(0d))
            {
                width = (line.GetPoint(0) - line.GetPoint(1)).Length * 0.02f;
                if (width > 0.01) width = 0.1f;
                if (width < 0.004) width = 0.001f;
            }
            width /= 2;
            List<Double3m> segments = new List<Double3m>();
            mesh ??= new MeshGeometry3D();
            for (float T = 0; T <= 1; T += SegmentResolution)
            {

                segments.Add(line.GetPoint(T));
            }
            var segmentEnumerator = segments.GetEnumerator();
            segmentEnumerator.MoveNext();
            Double3m previousPoint = segmentEnumerator.Current;
            int index = mesh.Positions.Count;
            while (segmentEnumerator.MoveNext())
            {
                mesh.Positions.Add(As3D(previousPoint - width));
                mesh.Positions.Add(As3D(previousPoint + width));
                mesh.Positions.Add(As3D(segmentEnumerator.Current));
                mesh.TriangleIndices.Add(index++);
                mesh.TriangleIndices.Add(index++);
                mesh.TriangleIndices.Add(index++);

                mesh.Positions.Add(As3D(segmentEnumerator.Current - width));
                mesh.Positions.Add(As3D(segmentEnumerator.Current + width));
                mesh.Positions.Add(As3D(previousPoint));
                mesh.TriangleIndices.Add(index++);
                mesh.TriangleIndices.Add(index++);
                mesh.TriangleIndices.Add(index++);
                //Debug.WriteLine(Math.Pow(previousPoint.X, 2) + Math.Pow(previousPoint.Y ,2));
                previousPoint = segmentEnumerator.Current;
            }


            return mesh;
        }
        /// <summary>
        /// generate a bounding box mesh from <paramref name="line"/>
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static MeshGeometry3D MeshBoundingBoxes(MeshGeometry3D mesh, Curve line)
        {
            mesh ??= new MeshGeometry3D();
            foreach (var box in line.BoundingBoxes)
            {
                mesh = MeshBox(mesh, box);
            }
            return mesh;
        }
        /// <summary>
        /// Generate a mesh from <paramref name="box"/> and add it to <paramref name="mesh"/>
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="box"></param>
        /// <returns></returns>
        public static MeshGeometry3D MeshBox(MeshGeometry3D mesh, (Double3m, Double3m) box)
        {
            int index = mesh.Positions.Count;

            foreach (Double3m v in FireAxe.FireMath.BoundingBoxes.Cornered(box))
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
        /// <summary>
        /// add triangle from to existing mesh.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        public static void addTriangle(MeshGeometry3D mesh, Point3D p1, Point3D p2, Point3D p3)
        {
            int index1 = addPoint(mesh.Positions, p1);
            int index2 = addPoint(mesh.Positions, p2);
            int index3 = addPoint(mesh.Positions, p3);
            mesh.TriangleIndices.Add(index1);
            mesh.TriangleIndices.Add(index2);
            mesh.TriangleIndices.Add(index3);
        }
        /// <summary>
        /// add point to point collection ?
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="point"></param>
        /// <returns></returns>
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
        /// <summary>
        /// lazy returns <see cref="Point3D"/> version of <paramref name="point"/>.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point3D As3D(Double3m point) => new Point3D(point.X, point.Y, point.Z);

    }
}

using FireAxe.Models.GeometryFormats;

using System.Diagnostics;
using System.Globalization;

namespace FireAxe.Models.Construction
{
    public class Geometry
    {
        public List<Triangle> triangles;

        public List<Double3m> vertices;
        public List<int> indices;
        public void LiftZ()
        {
            double min = double.MaxValue;
            foreach (Triangle triangle in triangles)
            {
                if (triangle.v1.Z < min) min = triangle.v1.Z;
                if (triangle.v2.Z < min) min = triangle.v2.Z;
                if (triangle.v3.Z < min) min = triangle.v3.Z;
            }
            Double3m direction = new Double3m(0, 0, -min);
            foreach (Triangle triangle in triangles)
            {
                triangle.TransForm(direction);
            }

        }
        public void CalculateIndices()
        {
            vertices = new List<Double3m>();
            indices = new List<int>();
            triangles = triangles.OrderBy(x => x.v1.LengthSquared).ToList();
            int blockSize = 8192;
            for (int i = 0; i < triangles.Count; i += blockSize)
            {
                if (i + blockSize > triangles.Count) blockSize = triangles.Count - i;
                CalculateIndices(triangles.GetRange(i, blockSize));
            }
            Debug.WriteLine($"Loaded STL, vertex ratio {(double)vertices.Count / (double)(triangles.Count * 3d)}");
        }
        public void CalculateIndices(List<Triangle> triangles)
        {
            var compare = Double3m.Comparator;
            var tempVerts = new List<Double3m>();
            foreach (var triangle in triangles)
            {
                tempVerts.Add(triangle.v1);
                tempVerts.Add(triangle.v2);
                tempVerts.Add(triangle.v3);
            }
            tempVerts = tempVerts.Distinct(compare).OrderBy(x => x.Z).ToList();
            int tempt = 0;


            List<(Triangle, int, int, int, int)> trianglebag =
                triangles.Select<Triangle, (Triangle, int, int, int, int)>(
                    x => new(x,
                        -1,
                        -1,
                        -1,
                        tempt++))
                .ToList();


            var result = System.Threading.Tasks.Parallel.ForEach(
                 trianglebag, triangle =>
                 {
                     triangle.Item2 = tempVerts.FindIndex(x => compare.Equals(x, triangle.Item1.v1));
                     triangle.Item3 = tempVerts.FindIndex(x => compare.Equals(x, triangle.Item1.v2));
                     triangle.Item4 = tempVerts.FindIndex(x => compare.Equals(x, triangle.Item1.v3));
                     trianglebag[triangle.Item5] = triangle;
                 });

            while (!result.IsCompleted)
            {
                Task.Delay(trianglebag.Count / 10);

            }

            int startIndex = vertices.Count;
            vertices.AddRange(tempVerts);
            foreach (var triangle in trianglebag)
            {
                indices.Add(triangle.Item2 + startIndex);
                indices.Add(triangle.Item3 + startIndex);
                indices.Add(triangle.Item4 + startIndex);
            }
            if (indices.Contains(-1))
                Console.WriteLine("Vertice not found");

        }

        public ScalarField AsScalarField(double tolerance = 0.2)
        {
            return new ScalarField(vertices, tolerance);
        }
        public List<string> AsPointCloud()
        {
            List<string> result = new List<string>();
            result.Add("X;Y;Z");
            foreach(var point in vertices)
            {
                result.Add($"{point.X.ToString(CultureInfo.InvariantCulture)};{point.Y.ToString(CultureInfo.InvariantCulture)};{point.Z.ToString(CultureInfo.InvariantCulture)}");
            }

            return result;
        }
    }
}

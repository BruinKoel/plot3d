﻿using FireAxe.Models.GeometryFormats;

using System.Diagnostics;
using System.Globalization;

namespace FireAxe.Models.Construction
{
    /// <summary>
    /// ParentClass for triangulated mesh types, like STL
    /// </summary>
    public abstract class Geometry
    {
        public Double3m origin;
        public Double3m normal;


        public List<Triangle> triangles;

        public List<Double3m> vertices;
        public List<int> indices;
     
        public Geometry() { }
        public Geometry(List<Triangle> triangles, bool disjointed = false)
        {
            this.triangles = triangles;

            this.LiftZ();

            if (disjointed)
            {
                this.QuickIndex();
            }
            else
            {
                this.CalculateIndices();
            }
            
        }

        public abstract void RegenerateMesh();
        
        

        /// <summary>
        ///  makes sure all points are on Positive Z values.
        /// </summary>
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
        /// <summary>
        /// Quickly produces vertices and indices but has Poor  render performance.
        /// </summary>
        public void QuickIndex()
        {
            int index = 0;
            vertices= new List<Double3m>();
            indices= new List<int>();

            foreach (Triangle triangle in triangles)
            {
                vertices.Add(triangle.v1);
                vertices.Add(triangle.v2);
                vertices.Add(triangle.v3);

                indices.Add(index++);
                indices.Add(index++);
                indices.Add(index++);
            }
        }
        /// <summary>
        /// Calculate the Vertices and indices for this object so that it can be "efficiently" drawn.
        /// </summary>
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
        /// <summary>
        /// Calculates the Indices and Vertices collection for aa give list of triangles.
        /// </summary>
        /// <param name="triangles"></param>
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

        public void buildTrianglesFromIndices()
        {
            {
            triangles = new List<Triangle>();
            for (int i = 0; i < indices.Count; i += 3)
                {
                triangles.Add(new Triangle(vertices[indices[i]], vertices[indices[i + 1]], vertices[indices[i + 2]]));
            }
            }
        }

        /// <summary>
        /// returns a scalrfield representation of this Geometry object
        /// </summary>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public ScalarField AsScalarField(double tolerance = -1)
        {
            buildTrianglesFromIndices();
            return new ScalarField(triangles, tolerance);
        }
        /// <summary>
        /// Returns PointCloudStringRepresentation for CSV export, should find a better place for this or remove, who knows.
        /// </summary>
        /// <returns></returns>
        public List<string> AsVertexCloudStringCSV()
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

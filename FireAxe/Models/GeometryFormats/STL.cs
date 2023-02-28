using FireAxe.Models.Construction;

namespace FireAxe.Models.GeometryFormats
{
    public class STL : Geometry
    {
        public byte[] header;
        

        public STL(byte[] data)
        {

            header = data[..80];
            int triangleP = 84;
            //int expectedTriangles = data[BitConverter.ToInt32(data[80..84])];//bro, wtf
            triangles = new List<Triangle>();

            for (int i = 0; i < (data.Length - 84) / 50; i++)
            {
                triangles.Add(readTriangleBytes(data[triangleP..(triangleP += 50)]));
            }

            this.LiftZ();
            this.CalculateIndices();
        }
        public Triangle readTriangleBytes(byte[] data)
        {
            int pointer = 0;
            Triangle triangle = new Triangle();
            if (!BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < data.Length; i += 4)
                {
                    (data[i], data[i + 1]) = (data[i + 2], data[i + 3]);
                }
            }

            triangle.normal = new Double3m(
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]));

            triangle.v1 = new Double3m(
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]));

            triangle.v2 = new Double3m(
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]));

            triangle.v3 = new Double3m(
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]),
                BitConverter.ToSingle(data[(pointer)..(pointer += 4)]));




            triangle.count = BitConverter.ToUInt16(data[pointer..(pointer += 2)]);
            return triangle;
        }

        public float getMaxDistance(Double3m p1, Double3m p2)
        {
            float tempo = MathF.Abs(p1.X - p2.X);
            tempo += MathF.Abs(p1.Y - p2.Y);
            return tempo + MathF.Abs(p1.Z - p2.Z);
        }

        public override void RegenerateMesh()
        {
          
        }
    }
    
}
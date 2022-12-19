using Importer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace plot3d
{
    public class MeshOperator
    {
        public MeshGeometry3D mesh;
        private STL stl;

        public MeshOperator()
        {
            stl = new STL(File.ReadAllBytes(@"C:\Users\epice\Downloads\swingarm.stl"));
            this.MainThread = new Thread(new ThreadStart(MainLoop));
            this.mesh = new MeshGeometry3D();
            stl.CalculateIndices();

            
            mesh.TriangleIndices = new System.Windows.Media.Int32Collection(stl.indices);
            mesh.Positions = new Point3DCollection(stl.vertices.OrderBy(x => x.Key).Select(x => x.Value));
            mesh.Normals = new Vector3DCollection(stl.triangles.Select(x => x.normal));



            tick = 0;
            MainThread.Start();
        }
        public delegate void Tick(int tick);

        public event Tick TicKTock;

        private int tick;
        public void RaiseMyEvent()
        {
            // Your logic
            if (TicKTock != null)
            {
                TicKTock(tick++);
            }
        }

        public Thread MainThread
        {
            get;
        }

        private void MainLoop()
        {
            while (true)
            {
                RaiseMyEvent();
                Thread.Sleep(50);
            }
        }

    }
}

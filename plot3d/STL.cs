using FireAxe.Models.GeometryFormats;
using plot3d;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Importer.Models
{
    public class STL : FireAxe.Models.GeometryFormats.STL
    {
        public List<int> visualIndices;
        public Dictionary<int, Point3D> visualVertecis;

        public STL(byte[] data) : base(data)
        {
            this.visualIndices = new List<int>();
            this.visualVertecis = new Dictionary<int, Point3D>();
        }

        
        //Debug.WriteLine(" ");

    }


}



using FireAxe.Models.GeometryFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.Construction
{
    public  class Form : Geometry
    {
        public Form() { }
        public Form(List<Triangle> triangles, bool disjointed = false)
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

        public override void RegenerateMesh()
        {
            
        }
    }
}

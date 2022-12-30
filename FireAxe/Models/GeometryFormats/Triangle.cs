﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireAxe.Models.GeometryFormats
{

    public class Triangle
    {
        public Double3m normal;
        public Double3m v1;
        public Double3m v2;
        public Double3m v3;
        public UInt16 count;
        public Triangle()
        {
            normal = new Double3m();
            v1 = new Point();
            v2 = new Point();
            v3 = new Point();
            //VSort();
        }
        public void TransForm(Double3m direction)
        {
            v1 += direction;
            v2 += direction;
            v3 += direction;
        }
        public void ZSort()
        {
            if(v1.Z >  v2.Z) (v1, v2) = (v2, v1);
            if(v1.Z > v3.Z) (v1, v2, v3) = (v3,v1, v2);
            if(v2.Z > v3.Z) (v2, v3) = (v3, v2);
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
                    (this.v1, this.v3) = (this.v3, this.v1);
                }
                else
                {
                    (this.v1, this.v3) = (this.v3, this.v1);
                }
                return;
            }

        }

    }
}

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
        }

        public Triangle(Double3m v1, Double3m v2, Double3m v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            CalculateNormal();
        }
        /// <summary>
        /// Returns the Centroid of the Triangle
        /// </summary>
        public Double3m Centroid
        {
            get
            {
                return (v1 + v2 + v3) / 3;
            }
        }
        /// <summary>
        /// Scales the triangle
        /// </summary>
        /// <param name="factor"></param>
        public void Scale(Double3m factor)
        {
            Double3m cent = Centroid;
            v1 += factor * (v1 - cent);
            v2 += factor * (v2 - cent);
            v3 += factor * (v3 - cent);
        }
        /// <summary>
        /// Calculates the Vector Normal to the Face of the Triangle. I think i did the right hand rule right, haven't checked.
        /// </summary>
        public void CalculateNormal()
        {
            normal = ((v2 - v1) ^ (v3 - v1)).Normal;
        }
        /// <summary>
        /// moves the triangle by <paramref name="direction"/>
        /// </summary>
        /// <param name="direction"></param>
        public void TransForm(Double3m direction)
        {
            v1 += direction;
            v2 += direction;
            v3 += direction;
        }
        /// <summary>
        /// Reordes the Vertices based on Z value
        /// </summary>
        public void ZSort()
        {
            if (v1.Z > v2.Z) (v1, v2) = (v2, v1);
            if (v1.Z > v3.Z) (v1, v2, v3) = (v3, v1, v2);
            if (v2.Z > v3.Z) (v2, v3) = (v3, v2);
        }
        public void VSort()
        {
            float p1 = 0;
            float p2 = 0;
            float p3 = 0;

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

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

        public Double3m Centroid
        {
            get
            {
                return (v1 + v2 + v3) / 3;
            }
        }

        public void Scale(Double3m factor)
        {
            Double3m cent = Centroid;
            v1 += factor * (v1 - cent);
            v2 += factor * (v2 - cent);
            v3 += factor * (v3 - cent);
        }

        public void CalculateNormal()
        {
            normal = (v2 - v1) ^ (v3 - v1).Normal;
        }

        public void TransForm(Double3m direction)
        {
            v1 += direction;
            v2 += direction;
            v3 += direction;
        }
        public void ZSort()
        {
            if (v1.Z > v2.Z) (v1, v2) = (v2, v1);
            if (v1.Z > v3.Z) (v1, v2, v3) = (v3, v1, v2);
            if (v2.Z > v3.Z) (v2, v3) = (v3, v2);
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

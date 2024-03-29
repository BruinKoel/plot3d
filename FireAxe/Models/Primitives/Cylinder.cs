﻿using FireAxe.Models.Curves;

namespace FireAxe.Models.Primitives
{
    public class Cylinder : Construction.Geometry
    {
        public Round baseCircle;

        public double height;

        public Double3m normal
        {
            get { return baseCircle.normal; }
            set { baseCircle.normal = value; }
        }
        public double radius
        {
            get { return baseCircle.radius; }
            set { baseCircle.radius = value; }
        }

        public Cylinder(Double3m origin, double radius, double height, Double3m normalPlane)
            
        {
            this.baseCircle ??= new Round(0, normalPlane, 1);
            this.height = height;
            RegenerateMesh();
        }

        public Cylinder(Double3m origin, double radius, double height)
        {

            this.baseCircle ??= new Round(0, new(0, 0, 1), 1);
            this.height = height;
            RegenerateMesh();
        }

        public override void RegenerateMesh()
        {
            triangles = new List<GeometryFormats.Triangle>();
            var offset = height * baseCircle.normal;
            var previousPoint = baseCircle.GetPoint(0);
            for (double t = 1d / 64d; t <= 1d; t += 1d / 64d)
            {
                var point = baseCircle.GetPoint(t);
                triangles.Add(new GeometryFormats.Triangle(point, previousPoint, point + offset));

                triangles.Add(new GeometryFormats.Triangle(previousPoint, previousPoint + offset, point + offset));
                previousPoint = point;
            }
            CalculateIndices();

        }
    }

}

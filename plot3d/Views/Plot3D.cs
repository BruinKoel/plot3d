﻿using FireAxe.Models;
using plot3d;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace plot3d.Views
{
    public class Plot3D : Viewport3D
    {
        private bool mousedown;

        Viewport3D viewPort;

        PerspectiveCamera camera;
        Model3DGroup group;

        public Plot3D()
        {




            camera = new PerspectiveCamera()
            {
                FieldOfView = 60,
                LookDirection = new Vector3D(1, 1, 1),
                UpDirection = new Vector3D(0, 0, 1),
                Position = new Point3D(-1, -1, 1)
            };
            group = new Model3DGroup() { Children = { new AmbientLight(Colors.White) } };
            viewPort = new Viewport3D()
            {
                Camera = camera,
                Children = { new ModelVisual3D() { Content = group } }
            };
            AddVisualChild(viewPort);

        }
        public void SetCamera(FireAxe.Models.Double3m pos)
        {
            this.camera.Position = Meshify.As3D(pos);
        }
        public void SetCamera(FireAxe.Models.Double3m pos, Double3m direction)
        {
            this.camera.Position = Meshify.As3D(pos);
            this.camera.LookDirection = (Vector3D)Meshify.As3D(direction.Normal);
        }

        public void Clear()
        {
            foreach (var model in group.Children.Where(x => x.GetType() == typeof(GeometryModel3D)).ToArray())
            {
                group.Children.Remove(model);
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            Console.WriteLine("Kek");

        }

        public void Plot3D_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Mouse.Capture(this);
        }

        private System.Windows.Point mouseTrail;

        public void Plot3D_MouseMove(object sender, MouseEventArgs e)
        {


            var mousepos = e.GetPosition(this);
            if (e.LeftButton == MouseButtonState.Released)
            {
                mouseTrail = mousepos;
                //Task.Delay(20);
                return;
            }

            Rotate((mousepos.X - mouseTrail.X));
            RotateVertical((mousepos.Y - mouseTrail.Y));

            mouseTrail = e.GetPosition(this);
        }

        public void Plot3D_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.S:

                    MoveForward(-movementspeed);
                    break;

                case Key.W:

                    MoveForward(movementspeed);
                    break;
                case Key.D:

                    MoveSideways(-movementspeed);
                    break;

                case Key.A:

                    MoveSideways(movementspeed);
                    break;

                case Key.V:

                    MoveUp(movementspeed);
                    break;

                case Key.LeftCtrl:
                    MoveUp(-movementspeed); ;
                    break;

            }
        }
        public void Rotate(double d)
        {
            double u = 0.3;
            double angleD = u * -d;
            PerspectiveCamera camera = (PerspectiveCamera)this.camera;
            Vector3D lookDirection = camera.LookDirection;

            var m = new Matrix3D();
            m.Rotate(new Quaternion(camera.UpDirection, -angleD)); // Rotate about the camera's up direction to look left/right
            this.camera.LookDirection = m.Transform(camera.LookDirection);
        }

        public void RotateVertical(double d)
        {
            double u = 0.3;
            double angleD = u * d;
            PerspectiveCamera camera = (PerspectiveCamera)this.camera;
            Vector3D lookDirection = camera.LookDirection;

            // Cross Product gets a vector that is perpendicular to the passed in vectors (order does matter, reverse the order and the vector will point in the reverse direction)
            var cp = Vector3D.CrossProduct(camera.UpDirection, lookDirection);
            cp.Normalize();

            var m = new Matrix3D();
            m.Rotate(new Quaternion(cp, -angleD)); // Rotate about the vector from the cross product
            this.camera.LookDirection = m.Transform(camera.LookDirection);
        }
        public void MoveForward(double d)
        {
            double u = 0.05;
            PerspectiveCamera camera = (PerspectiveCamera)this.camera;
            Vector3D lookDirection = camera.LookDirection;
            Point3D position = camera.Position;

            lookDirection.Normalize();
            position = position + u * lookDirection * d;

            camera.Position = position;
        }
        public void MoveSideways(double d)
        {
            double u = -0.05;
            PerspectiveCamera camera = (PerspectiveCamera)this.camera;
            Vector3D lookDirection = Vector3D.CrossProduct(camera.LookDirection, new Vector3D(0, 0, 1));
            Point3D position = camera.Position;

            lookDirection.Normalize();
            position = position + u * lookDirection * d;

            camera.Position = position;
        }
        public void MoveUp(double d)
        {
            double u = 0.05;
            PerspectiveCamera camera = (PerspectiveCamera)this.camera;
            Vector3D lookDirection = new Vector3D(0, 0, 1);
            Point3D position = camera.Position;

            lookDirection.Normalize();
            position = position + u * lookDirection * d;

            camera.Position = position;
        }

        public void deleteModels()
        {
            group.Children.Clear();
        }
        Random random = new Random();
        public Brush GoledenColor()
        {

            byte[] bytes = new byte[4];
            random.NextBytes(bytes);
            Brush brush = new SolidColorBrush(Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]));
            return brush;

        }
        public void addModel(MeshGeometry3D mesh)
        {
            //camera.Position = new Point3D(mesh.Bounds.Location.X + mesh.Bounds.SizeX * 1.6,
            //    mesh.Bounds.Location.Y + mesh.Bounds.SizeY * 1.6,
            //   mesh.Bounds.Location.Z + mesh.Bounds.SizeZ * 1.6);


            var model = new GeometryModel3D()
            {
                Geometry = mesh,
                Material = new DiffuseMaterial(GoledenColor()),
                BackMaterial = new DiffuseMaterial(GoledenColor()),
            };
            group.Children.Add(model);

            //viewPort.UpdateLayout();

            //var line = new Line() { X1 = 0, X2 = 0, Y1 = 0, Y2 = 8, Stroke = Brushes.Blue, StrokeThickness = 4 };
            //var line2d = new Viewport2DVisual3D() {
            //    Geometry = plane.Geometry,
            //    Visual = line,
            //    Material = new DiffuseMaterial(Brushes.Blue)
            //};
            //line2d.Material.SetValue(Viewport2DVisual3D.IsVisualHostMaterialProperty, true);
            //viewPort.Children.Add(line2d);
        }

        double movementspeed = 10;
        public void Plot3D_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            movementspeed = movementspeed + (movementspeed * e.Delta / 1000);
            if (movementspeed > 1000) movementspeed = 1000;
            else if (movementspeed < 1) movementspeed = 1;
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            viewPort.Width = finalSize.Width;
            viewPort.Height = finalSize.Height;
            viewPort.Measure(finalSize);
            viewPort.Arrange(new Rect(viewPort.DesiredSize));
            return finalSize;
        }
        public void setCamera(Point3D location)
        {
            this.camera.Position = location;
        }

        protected override Visual GetVisualChild(int index) => viewPort;
        protected override int VisualChildrenCount => 1;
    }
}
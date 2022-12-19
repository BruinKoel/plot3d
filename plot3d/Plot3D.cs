using plot3d;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

public class Plot3D : Viewport3D
{
    private bool mousedown;

    Viewport3D viewPort;

    PerspectiveCamera camera;
    Model3DGroup group;
    Point3DAnimationUsingKeyFrames positionAnim;
    Vector3DAnimationUsingKeyFrames lookAnim;
    public Plot3D()
    {




        camera = new PerspectiveCamera()
        {
            FieldOfView = 60,
            LookDirection = new Vector3D(1, 1, 1),
            UpDirection = new Vector3D(0, 1, 0),
            Position = new Point3D(-1,-1,1)
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

    public void Clear()
    {
        group.Children.Clear();
    }

    protected override void OnRender(DrawingContext dc)
    {
        Console.WriteLine("Kek");

    }

    public void Plot3D_MouseDown(object sender, MouseButtonEventArgs e)
    {
        //Mouse.Capture(this);
    }

    private Point mouseTrail;

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
            case Key.NumPad6:

                Rotate(10);
                break;

            case Key.NumPad4:

                Rotate(-10);
                break;

            case Key.S:

                MoveForward(-movementspeed);
                break;

            case Key.W:

                MoveForward(movementspeed);
                break;
            case Key.D:
                Rotate(-1);
                MoveSideways(-10);
                break;

            case Key.A:
                Rotate(1);
                MoveSideways(10);
                break;

            case Key.PageUp:

                RotateVertical(10);
                break;

            case Key.PageDown:

                RotateVertical(-10);
                break;

        }
    }
    public void Rotate(double d)
    {
        double u = 0.05;
        double angleD = u * -d;
        PerspectiveCamera camera = (PerspectiveCamera)this.camera;
        Vector3D lookDirection = camera.LookDirection;

        var m = new Matrix3D();
        m.Rotate(new Quaternion(camera.UpDirection, -angleD)); // Rotate about the camera's up direction to look left/right
        this.camera.LookDirection = m.Transform(camera.LookDirection);
    }

    public void RotateVertical(double d)
    {
        double u = 0.05;
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
        double u = 0.05;
        PerspectiveCamera camera = (PerspectiveCamera)this.camera;
        Vector3D lookDirection = camera.LookDirection;
        Point3D position = camera.Position;

        (lookDirection.X, lookDirection.Y, lookDirection.Z) = (lookDirection.Z, lookDirection.X, lookDirection.Y);

        lookDirection.Normalize();

        position = position + u * lookDirection * d;

        camera.Position = position;
    }

    public void deleteModels()
    {
        group.Children.Clear();
    }
    public void addModel(MeshGeometry3D mesh)
    {
        //camera.Position = new Point3D(mesh.Bounds.Location.X + mesh.Bounds.SizeX * 1.6,
        //    mesh.Bounds.Location.Y + mesh.Bounds.SizeY * 1.6,
        //   mesh.Bounds.Location.Z + mesh.Bounds.SizeZ * 1.6);


        var model = new GeometryModel3D()
        {
            Geometry = mesh,
            Material = new DiffuseMaterial(Brushes.DarkBlue),
            BackMaterial = new DiffuseMaterial(Brushes.Red),
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
    double f(double x, double z) => x * x + z * z;

    protected override Size ArrangeOverride(Size finalSize)
    {
        viewPort.Width = finalSize.Width;
        viewPort.Height = finalSize.Height;
        viewPort.Measure(finalSize);
        viewPort.Arrange(new Rect(viewPort.DesiredSize));
        return finalSize;
    }

    protected override Visual GetVisualChild(int index) => viewPort;
    protected override int VisualChildrenCount => 1;
}
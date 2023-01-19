using FireAxe.FireMath;
using FireAxe.Models;
using FireAxe.Models.Curves;
using Importer.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;

namespace plot3d
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Plot3D plot
        {
            get;
            set;
        }
        MeshOperator meshOperator;
        public MainWindow()
        {

            plot = new Plot3D();
            DataContext = plot;
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.MouseWheel += plot.Plot3D_MouseWheel;
        }



        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //meshOperator = new MeshOperator();
            //plot.addModel(meshOperator.mesh);


            Plot3DBorders.MouseMove += plot.Plot3D_MouseMove;
            Plot3DBorders.MouseDown += plot.Plot3D_MouseDown;
            KeyDown += plot.Plot3D_KeyDown;
            //Plot3DBorders.MouseUp += (x, e) => plot.RaiseEvent(e);


            Plot3DBorders.Child = plot;

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(1000 * 150);
            dispatcherTimer.Start();



        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            MoveAlongRandomSpline();
        }
        Random random = new Random();
        private void button_Click(object sender, RoutedEventArgs e)
        {
            //DisableTrack = !DisableTrack;
            //plot.addModel(Meshify.MeshCurve(new CubicSpline( randomPoints()), 0.002));
            var randomPoints = Enumerable.Range(0, 100).Select(x => this.randomPoints());

            //var temp = new CubicSpline(randomPoints);
            plot.addModel(Meshify.MeshCurve(randomPoints.Select(x => new CubicSpline(x)), 0.01));
            //plot.addModel(Meshify.MeshCurve(new LinearSpline(randomPoints), 0.01));
            //plot.addModel(Meshify.MeshBoundingBoxes(temp));

            //plot.addModel(Meshify.MeshBoundingBoxes(new Straigth(randomPoints(10).First(), randomPoints(100).Last())));

        }

        bool DisableTrack = true;
        CubicSpline track;
        double trackT;

        private void MoveAlongRandomSpline(double size = 0.00001)
        {
            if (DisableTrack)
            {
                track = null;
                trackT = 0;
                return;
            }
            track ??= new CubicSpline(randomPoints());

            plot.SetCamera(track.GetPoint(trackT += size));
            if (trackT >= 1) DisableTrack = !DisableTrack;
        }
        private List<Double3m> randomPoints(int count = 100)
        {
            List<Double3m> points = new List<Double3m>();
            points.Add(new Double3m());
            for (int i = 1; i < count; i++)
            {
                points.Add(points[i - 1] + new FireAxe.Models.Double3m()
                {
                    X = (random.NextDouble() - 0.2),
                    Y = (random.NextDouble() - 0.2),
                    Z = (random.NextDouble() - 0.2)
                });
            }

            return points;
        }
        private List<Double3m> circlePoints(int count = 100)
        {
            double tempZ = random.NextDouble() * 10;
            List<Double3m> points = new List<Double3m>();
            for (int i = -1; i < count; i++)
            {
                points.Add(new FireAxe.Models.Double3m()
                {
                    X = Math.Cos(((double)i / (double)count) * 2 * Math.PI),
                    Z = Math.Sin(((double)i / (double)count) * 2 * Math.PI),
                    Y = kok
                });
            }
            points.Add(points.First());


            return points;
        }

        private void ClearButton(object sender, RoutedEventArgs e)
        {

            plot.Clear();
        }

        private void SimpleSplineButton(object sender, RoutedEventArgs e)
        {
            plot.addModel(Meshify.MeshCurve(new SimpleSpline(randomPoints()), 0.02));
        }

        private void TrackCameraButton(object sender, RoutedEventArgs e)
        {
            DisableTrack = !DisableTrack;
        }
        int kok = 0;
        private void CubicCircleButton(object sender, RoutedEventArgs e)
        {
            plot.addModel(Meshify.MeshBoundingBoxes(new MeshGeometry3D(), new CubicSpline(circlePoints(kok++))));
        }

        private void LoadSTLButton(object sender, RoutedEventArgs e)
        {
            double offsett = random.NextDouble() * 5;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                STL stl = new STL(System.IO.File.ReadAllBytes(openFileDialog.FileName));

                MeshGeometry3D mesh = new MeshGeometry3D();



                mesh.TriangleIndices = new System.Windows.Media.Int32Collection(stl.indices);
                mesh.Positions = new Point3DCollection(stl.vertices.Select(x => Meshify.As3D(x)));
                //mesh.Normals = new Vector3DCollection(stl.triangles.Select(x => (Vector3D)Meshify.As3D(x.normal)));
                plot.addModel(Meshify.MeshWireframe(stl));

                plot.addModel(mesh);
            }

        }

        private void SliceModelButton(object sender, RoutedEventArgs e)
        {
            double layerheight = 0.5;
            double offsett = random.NextDouble() * 5;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Construct stl = new Construct(new STL(System.IO.File.ReadAllBytes(openFileDialog.FileName)));

                plot.addModel(Meshify.MeshCurve(Curves.LinearSimplify(stl.Slice(layerheight)), layerheight));

                //foreach (var slice in FireAxe.FireMath.Curves.CubicSimplify(stl.Slice(layerheight)))
                //{
                // plot.addModel(Meshify.MeshCurve(slice,layerheight));
                //}
                //MeshGeometry3D mesh = new MeshGeometry3D();
                //mesh.TriangleIndices = new System.Windows.Media.Int32Collection(stl.geometry.indices);
                //mesh.Positions = new Point3DCollection(stl.geometry.vertices.Select(x => Meshify.As3D(x)));
                //mesh.Normals = new Vector3DCollection(stl.triangles.Select(x => (Vector3D)Meshify.As3D(x.normal)));


                //plot.addModel(mesh);

                plot.addModel(Meshify.Mesh(stl.geometry));

                plot.setCamera(Meshify.As3D(stl.geometry.triangles.First().v3));
            }
        }

        private void ExportAsCSVButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Construct stl = new Construct(new STL(System.IO.File.ReadAllBytes(openFileDialog.FileName)));

                File.WriteAllLines(openFileDialog.FileName + ".csv", stl.geometry.AsVertexCloudStringCSV());


            }
        }

        private void ScalarfieldButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Construct stl = new Construct(new STL(System.IO.File.ReadAllBytes(openFileDialog.FileName)));

                var field =stl.geometry.AsScalarField(0.4);
                field.Boolean();
                field.RayFill();
                plot.addModel(Meshify.MeshScalarField(field));

            }
        }
        private void HollowScalarfieldButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Construct stl = new Construct(new STL(System.IO.File.ReadAllBytes(openFileDialog.FileName)));

                var field = stl.geometry.AsScalarField(0.4);
                
                plot.addModel(Meshify.MeshScalarField(field));

            }
        }

    }
}

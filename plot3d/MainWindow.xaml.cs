using FireAxe.Models;
using FireAxe.Models.Curves;
using Importer.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
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

            meshOperator = new MeshOperator();
            //plot.addModel(meshOperator.mesh);


            Plot3DBorders.MouseMove += plot.Plot3D_MouseMove;
            Plot3DBorders.MouseDown += plot.Plot3D_MouseDown;
            KeyDown += plot.Plot3D_KeyDown;
            //Plot3DBorders.MouseUp += (x, e) => plot.RaiseEvent(e);
            

            Plot3DBorders.Child = plot;

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(1000*150);
            dispatcherTimer.Start();



        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            randomline();
        }
        Random random = new Random();
        private void button_Click(object sender, RoutedEventArgs e)
        {
            //DisableTrack = !DisableTrack;
            //plot.addModel(Meshify.MeshCurve(new CubicSpline( randomPoints()), 0.002));
            plot.addModel(Meshify.MeshCurve(new CubicSpline(randomPoints()), 0.004));

        }
        FireAxe.Models.Double3m previousPoint = new FireAxe.Models.Point();
        bool DisableTrack = true;
        CubicSpline track;
        double trackT;

        private void randomline(double size = 0.00001)
        {
            if (DisableTrack)
            {
                track = null;
                trackT = 0;
                return;
            }
            track ??= new CubicSpline(randomPoints());

            plot.SetCamera(track.GetPoint( trackT += size));
            if (trackT >= 1) DisableTrack = !DisableTrack;
        }
        private List<Double3m> randomPoints(int count = 100)
        {
            List<Double3m> kek = new List<Double3m>();
            kek.Add(new Double3m());
            for(int i = 1; i < count; i++)
            {
                kek.Add(kek[i - 1] + new FireAxe.Models.Double3m()
                {
                    X = (random.NextDouble() - 0.2),
                    Y = (random.NextDouble() - 0.2),
                    Z = (random.NextDouble() - 0.2)
                });
            }

            return kek;
        }
        private List<Double3m> circlePoints(int count = 100)
        {
            double tempZ = random.NextDouble()*100;
            List<Double3m> kek = new List<Double3m>();
            for (int i = -1; i < count; i++)
            {
                kek.Add( new FireAxe.Models.Double3m()
                {
                    X = Math.Cos(((double)i/ (double)count) * 2*Math.PI),
                    Y = Math.Sin(((double)i / (double)count) * 2 * Math.PI),
                    Z = tempZ
                });
            }
            kek.Add(kek.First());
            

            return kek;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            plot.Clear();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            plot.addModel(Meshify.MeshCurve(new SimpleSpline(randomPoints()), 0.02));
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            DisableTrack = !DisableTrack;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            plot.addModel(Meshify.MeshCurve(new CubicSpline(circlePoints(random.Next(4,100))), 0.004));
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            double offsett = random.NextDouble() * 5;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                STL stl = new STL(System.IO.File.ReadAllBytes(openFileDialog.FileName));
                
                MeshGeometry3D mesh = new MeshGeometry3D();
                stl.CalculateIndices();
                

                mesh.TriangleIndices = new System.Windows.Media.Int32Collection(stl.indices);
                mesh.Positions = new Point3DCollection(stl.vertices.OrderBy(x => x.Key).Select(x => new Point3D(
                    x.Value.X + offsett,
                    x.Value.Y + offsett,
                    x.Value.Z + offsett)));
                mesh.Normals = new Vector3DCollection(stl.triangles.Select(x => x.normal));


                plot.addModel(mesh);
            }
                
        }
    }
}

using FireAxe.Models;
using FireAxe.Models.Curves;
using plot3d.Views;
using System;
using System.Collections.Generic;


namespace plot3d.ViewModels
{
    internal class ConstructionViewModel
    {
        public Plot3D plot3D { get; }
        public FeatureBrowser featureBrowser { get; }

        public Construct construct;

        public ConstructionViewModel(Plot3D plot3D, FeatureBrowser featureBrowser)
        {
            this.plot3D = plot3D ?? throw new ArgumentNullException(nameof(plot3D));
            this.featureBrowser = featureBrowser ?? throw new ArgumentNullException(nameof(featureBrowser));

            Loaded();
        }

        public ConstructionViewModel()
        {
            this.plot3D ??= new Plot3D();
            this.featureBrowser ??= new FeatureBrowser();


            Loaded();
        }
        private System.Random random = new System.Random();
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

        public void Loaded()
        {
            this.construct = featureBrowser.baseConstruct;
            plot3D.SetCamera(new Double3m(10, 10, 10), new Double3m(-1, -1, -1));

            plot3D.addModel(Meshify.MeshCurve(new SimpleSpline(randomPoints()), 0.02));

            featureBrowser.ConstructUpdated += Refresh;
        }
        public void Refresh()
        {
            DrawConstruct(construct);
        }
        public void DrawConstruct(Construct construct)
        {
            if (construct.children != null)
            {


                foreach (Construct construct1 in construct.children)
                {
                    DrawConstruct(construct1);
                }
            }
            if (construct.geometry != null)
            {
                plot3D.addModel(Meshify.Mesh(construct.geometry));
            }

        }
    }
}

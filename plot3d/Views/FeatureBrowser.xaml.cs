using FireAxe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FireAxe.Models.Primitives;

namespace plot3d.Views
{

    public delegate void UpdatedConstruct();

    /// <summary>
    /// Interaction logic for FeatureBrowser.xaml
    /// </summary>
    public partial class FeatureBrowser : UserControl
    {
        public Construct baseConstruct;
        public event UpdatedConstruct ConstructUpdated;

        public FeatureBrowser()
        {
           
            baseConstruct= new Construct();
            InitializeComponent();
        }


        private void TreeRightMouseDown(object sender, RoutedEventArgs e)
        {
            //this.ContextMenu.IsOpen= true;

        }
        private void AddCylinder(object sender, RoutedEventArgs e)
        {
            baseConstruct.Add(new Construct(new Cylinder(0,10,10)));
            ConstructUpdated.Invoke();
            
        }
        private void RemoveSelected(object sender, RoutedEventArgs e)
        {

        }
    }
}

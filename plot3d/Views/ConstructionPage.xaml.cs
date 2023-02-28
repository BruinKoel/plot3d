using plot3d.ViewModels;
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

namespace plot3d.Views
{
    /// <summary>
    /// Interaction logic for ConstructionPage.xaml
    /// </summary>
    public partial class ConstructionPage : UserControl
    {
        public ConstructionPage()
        {
            
            InitializeComponent();
            DataContext = new ConstructionViewModel(ViewPort,FeaturePort);
        }
    }
}

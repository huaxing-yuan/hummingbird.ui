using System;
using System.Collections.Generic;
using System.Data;
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

namespace Hummingbird.UI.Demo
{
    /// <summary>
    /// Interaction logic for ViewNativeControls.xaml
    /// </summary>
    public partial class ViewNativeControls : ModernContent
    {
        public ViewNativeControls()
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("First Name"));
            dt.Columns.Add(new DataColumn("Last Name"));
            dt.Rows.Add("Huaxing", "Yuan");
            dt.Rows.Add("", "");
            dgDemo.ItemsSource = dt.DefaultView;
        }
    }
}

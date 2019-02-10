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
using System.Windows.Shapes;

namespace Hummingbird.UI.Demo
{
    /// <summary>
    /// Interaction logic for WindowBasicWindow.xaml
    /// </summary>
    public partial class WindowBasicWindow : BasicWindow
    {
        public WindowBasicWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SideBarVisibility = Visibility.Visible;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInformation("Title of the information", "This is the content of the message to be shown");
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            this.ShowToastNotification("This is a toast notification", NotificationLevel.Information);
        }
    }
}

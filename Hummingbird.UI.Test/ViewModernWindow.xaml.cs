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

namespace Hummingbird.UI.Demo
{
    /// <summary>
    /// Interaction logic for ViewModernWindow.xaml
    /// </summary>
    public partial class ViewModernWindow : ModernContent
    {
        public ViewModernWindow()
        {
            InitializeComponent();
        }

        private void ModernSwitch_IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (msBorder.IsChecked)
            {
                this.ParentWindow.WindowStatus = WindowBorderType.Busy;
            }
            else
            {
                this.ParentWindow.WindowStatus = WindowBorderType.Normal;
            }
        }

        private void ModernSwitch2_IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (msBorder2.IsChecked)
            {
                this.ParentWindow.WindowStatus = WindowBorderType.Good;
            }
            else
            {
                this.ParentWindow.WindowStatus = WindowBorderType.Bad;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowBasicWindow wbw = new WindowBasicWindow();
            wbw.Show();
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

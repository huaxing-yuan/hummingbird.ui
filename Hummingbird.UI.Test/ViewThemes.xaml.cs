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
    /// Interaction logic for ViewThemes.xaml
    /// </summary>
    public partial class ViewThemes : ModernContent
    {
        Border previousTheme;

        public ViewThemes()
        {
            InitializeComponent();
        }

        private void borderColor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Border border = (sender as Border);
            string themeColor = border.Tag.ToString();

            borderPurple.BorderBrush = null;
            borderBlue.BorderBrush = null;
            if (previousTheme != null)
            {
                previousTheme.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
            string theme = themeColor;
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Hummingbird.UI;component/Styles/ColorShemes/" + theme + ".xaml") });

            border.BorderBrush = (Brush)FindResource("HighlightBrush");
            previousTheme = border;
            foreach (Window w in Application.Current.Windows)
            {
                if (w is BasicWindow bw)
                {
                    bw.UpdateIcon();
                    bw.UpdateBackground();
                }
            }
        }
    }
}

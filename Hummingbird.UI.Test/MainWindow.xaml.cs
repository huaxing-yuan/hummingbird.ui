/*
 * Created by SharpDevelop.
 * User: Y-HUAXING
 * Date: 11/25/2015
 * Time: 10:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Hummingbird.UI;

namespace Hummingbird.UI.Demo
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : ModernWindow
	{
		public Window1()
		{
			InitializeComponent();
		}
		void Button_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary(){ Source = new Uri("pack://application:,,,/Hummingbird.UI;component/Styles/ColorShemes/Dark.Blue.xaml", UriKind.RelativeOrAbsolute)});
		}
		void Button2_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary(){ Source = new Uri("pack://application:,,,/Hummingbird.UI;component/Styles/Sizes/LargeSize.xaml", UriKind.RelativeOrAbsolute)});
		}
		
		void ButtonLight_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary(){ Source = new Uri("pack://application:,,,/Hummingbird.UI;component/Styles/ColorShemes/Light.Blue.xaml", UriKind.RelativeOrAbsolute)});


        }
		
		void ButtonNormal_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary(){ Source = new Uri("pack://application:,,,/Hummingbird.UI;component/Styles/Sizes/DefaultSize.xaml", UriKind.RelativeOrAbsolute)});
		}

		void Button1Menu_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(MainMenu.Count.ToString());
			this.Content = new TextBlock() {Text = "Change"};
		}

        private void ModernWindow_AvatarClicked(object sender, MouseEventArgs e)
        {
            ShowMessageBox("Avatar clicked", "You have clicked the avatar icon");
        }
    }
}
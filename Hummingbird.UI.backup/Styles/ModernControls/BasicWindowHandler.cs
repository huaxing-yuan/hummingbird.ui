// <copyright file="BasicWindowHandler.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-10-8  00:41</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Windows.Input;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Controls;

namespace Hummingbird.UI.ModernControls
{
	/// <summary>
	/// Description of ModernWindowHandler.
	/// </summary>
	internal partial class BasicWindowHandler
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        void Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window window = Window.GetWindow(e.Source as DependencyObject);

            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            {
                ChangeWindowState(window);
            }
            if (e.ChangedButton == MouseButton.Left)
            {
                ReleaseCapture();
                SendMessage(new WindowInteropHelper(window).Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        void BtnHideSideBar_Click(object sender, RoutedEventArgs e)
        {
            BasicWindow window = (BasicWindow)Window.GetWindow(e.Source as DependencyObject);
            window.SideBarVisibility = Visibility.Collapsed;
        }

        static void ChangeWindowState(Window w)
        {
            if (w.WindowState == WindowState.Maximized)
            {
                w.WindowState = WindowState.Normal;
            }
            else if (w.WindowState == WindowState.Normal)
            {
                w.WindowState = WindowState.Maximized;
            }
        }
        void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(e.Source as DependencyObject);
            window.WindowState = WindowState.Minimized;
        }

        void MaximumButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(e.Source as DependencyObject);
            ChangeWindowState(window);
        }
        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(e.Source as DependencyObject);
            window.Close();
        }

        void btnCloseMessageBox_Click(object sender, RoutedEventArgs e)
        {
            BasicWindow window = Window.GetWindow(e.Source as DependencyObject) as BasicWindow;
            window.IsMaskVisible = false;
            window.IsMessageBoxVisible = false;
        }

    }


}

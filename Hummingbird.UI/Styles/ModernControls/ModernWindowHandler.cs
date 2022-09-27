// <copyright file="ModernWindowHandler.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-7-9  17:53</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Windows.Input;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Hummingbird.UI.ModernControls
{
    /// <summary>
    /// Description of ModernWindowHandler.
    /// </summary>
    internal partial class ModernWindowHandler
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        Storyboard HideContentStoryboard = null;

        public ModernWindowHandler()
        {

        }

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


        void Avatar_MouseClick(object sender, MouseEventArgs e)
        {
            ModernWindow window = Window.GetWindow(e.Source as DependencyObject) as ModernWindow;
            window.InvokeAvatarClicked(sender, e);
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

        void DetachButton_Click(object sender, RoutedEventArgs e)
        {
            ModernWindow window = Window.GetWindow(e.Source as DependencyObject) as ModernWindow;
            window.DetachContent();
        }

        void AttachButton_Click(object sender, RoutedEventArgs e)
        {
            ModernWindow window = Window.GetWindow(e.Source as DependencyObject) as ModernWindow;
            window.AttachContent();
        }


        void MaximumButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(e.Source as DependencyObject);
            ChangeWindowState(window);
        }


        void RegularViewButton_Click(object sender, RoutedEventArgs e)
        {
            ModernWindow window = Window.GetWindow(e.Source as DependencyObject) as ModernWindow;
            window.Style = (Style)window.FindResource("DefaultModernWindow");
        }

        void FocusedViewButton_Click(object sender, RoutedEventArgs e)
        {
            ModernWindow window = Window.GetWindow(e.Source as DependencyObject) as ModernWindow;
            window.Style = (Style)window.FindResource("FocusedModernWindow");
        }


        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(e.Source as DependencyObject);
            window.Close();
        }

        void BackstageButton_Click(object sender, RoutedEventArgs e)
        {
            ModernWindow window = Window.GetWindow(e.Source as DependencyObject) as ModernWindow;
            var template = window.Template;
            Border container = template.FindName("BackstageContainer", window) as Border;
            if (container.Visibility == Visibility.Collapsed)
            {
                container.Visibility = Visibility.Visible;
            }
            else
            {
                if (window.BackstagePanel != null)
                {
                    foreach (var item in window.BackstagePanel.Items)
                    {
                        if (item is TabItem tabItem && tabItem.Content is ModernContent mc && !mc.CanBeClosed())
                        {
                            return;
                        }
                    }
                }


                if (HideContentStoryboard == null)
                {
                    HideContentStoryboard = ((Storyboard)container.FindResource("HideContentStoryboard")).Clone();
                    HideContentStoryboard.Completed += (s, e2) => HideContentStoryboard_Completed(container, e2);
                }
                container.BeginStoryboard(HideContentStoryboard);
            }
        }

        private void HideContentStoryboard_Completed(object sender, EventArgs e)
        {
            (sender as Border).Visibility = Visibility.Collapsed;
        }


        void btnCloseMessageBox_Click(object sender, RoutedEventArgs e)
        {
            BasicWindow window = Window.GetWindow(e.Source as DependencyObject) as BasicWindow;
            window.IsMaskVisible = false;
            window.IsMessageBoxVisible = false;
        }
    }
}

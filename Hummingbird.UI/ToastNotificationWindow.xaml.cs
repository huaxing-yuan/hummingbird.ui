// <copyright file="ToastNotificationWindow.xaml.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-8-27  15:44</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Hummingbird.UI
{

    /// <summary>
    /// The standard ToastNotification
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    internal partial class ToastNotificationWindow : Window
    {
        static List<ToastNotificationWindow> activeToastNotifications = new List<ToastNotificationWindow>();
        private DispatcherTimer timer;
        int _timeout;
        Action callBackAction = null;


        Window parentWindow;
        /// <summary>
        /// Initializes a new instance of the <see cref="ToastNotificationWindow" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="information">The information.</param>
        /// <param name="level">The level.</param>
        /// <param name="CallBackAction">This action will be called if the user clicked on this Toast Notification</param>
        /// <param name="timeout">The timeout in second when the notification hides itself, default value is 10</param>
        internal ToastNotificationWindow(Window parent, string information, NotificationLevel level, Action CallBackAction = null, int timeout = 10)
        {
            InitializeComponent();
            parentWindow = parent;
            _timeout = timeout;
            this.Information = information;
            this.NotificationLevel = level;
            activeToastNotifications.Add(this);
            if (activeToastNotifications.Count > 5)
            {
                activeToastNotifications.RemoveAt(0);
            }
            callBackAction = CallBackAction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToastNotificationWindow"/> class.
        /// </summary>
        internal ToastNotificationWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the information to be shown
        /// </summary>
        /// <value>
        /// The information.
        /// </value>
        public string Information
        {
            get { return (string)GetValue(InformationProperty); }
            set { SetValue(InformationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Information.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The information property
        /// </summary>
        public static readonly DependencyProperty InformationProperty =
            DependencyProperty.Register("Information", typeof(string), typeof(ToastNotificationWindow), new PropertyMetadata());

        [StructLayout(LayoutKind.Sequential)]
        internal struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        // Make sure RECT is actually OUR defined struct, not the windows rect.
        private static Rect GetWindowRectangle(Window window)
        {
            GetWindowRect((new WindowInteropHelper(window)).Handle, out Rect rect);

            return rect;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double baseTop = 40;
            if (parentWindow.WindowState == WindowState.Maximized)
            {
                var rect = GetWindowRectangle(parentWindow);
                this.Left = rect.Right - this.ActualWidth - 10 - 16;
                baseTop += rect.Top;
            }
            else
            {
                this.Left = parentWindow.Left + parentWindow.ActualWidth - this.ActualWidth - 10;
                baseTop += parentWindow.Top;
            }

            foreach (var w in activeToastNotifications)
            {
                if (w == this) break;
                if (w.Top + w.ActualHeight + 10 > baseTop)
                {
                    baseTop = w.Top + w.ActualHeight + 10;
                }
            }
            this.Top = baseTop;


            timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, _timeout),
                IsEnabled = true,

            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Gets or sets the notification level.
        /// </summary>
        /// <value>
        /// The notification level.
        /// </value>
        public NotificationLevel NotificationLevel
        {
            get { return (NotificationLevel)GetValue(NotificationLevelProperty); }
            set { SetValue(NotificationLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotificationLevel.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The notification level property
        /// </summary>
        public static readonly DependencyProperty NotificationLevelProperty =
            DependencyProperty.Register("NotificationLevel", typeof(NotificationLevel), typeof(ToastNotificationWindow), new PropertyMetadata(NotificationLevel.Information, NotificationLevelChanged));

        private static void NotificationLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotificationLevel level = (NotificationLevel)e.NewValue;
            ToastNotificationWindow instance = d as ToastNotificationWindow;
            Brush brush = null;
            switch (level)
            {
                case NotificationLevel.Information:
                    brush = (Brush)instance.FindResource("HighlightDarkBrush");
                    instance.btnLevel.IconKey = "appbar_information";
                    break;
                case NotificationLevel.Warning:
                    brush = (Brush)instance.FindResource("AlertTextBrush");
                    instance.btnLevel.IconKey = "appbar_warning";
                    break;
                case NotificationLevel.Error:
                    brush = (Brush)instance.FindResource("BadTextBrush");
                    instance.btnLevel.IconKey = "appbar_warning_circle";
                    break;
                case NotificationLevel.Good:
                    brush = (Brush)instance.FindResource("GoodTextBrush");
                    instance.btnLevel.IconKey = "appbar_check";
                    break;

            }
            instance.mainBorder.BorderBrush = brush;
            instance.iconBorder.Background = brush;
        }

        private void AppbarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.IsEnabled = false;
                timer.Tick -= Timer_Tick;
                timer = null;
            }
            try
            {
                activeToastNotifications.Remove(this);
            }
            catch {
                //nothing to do here
            }
        }

        private void txtInformation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            callBackAction?.Invoke();
            this.Close();
        }
    }

    /// <summary>
    /// Level of the notification, colors and behaviors will be different according to the level.
    /// </summary>
    public enum NotificationLevel
    {
        /// <summary>
        /// The notification is an Information
        /// </summary>
        Information,

        /// <summary>
        /// The notification is a validation (will shows in green)
        /// </summary>
        Good,

        /// <summary>
        /// The notification is a Warning (will shows in yellow)
        /// </summary>
        Warning,
        /// <summary>
        /// The notification is an Error
        /// </summary>
        Error,
    }
}

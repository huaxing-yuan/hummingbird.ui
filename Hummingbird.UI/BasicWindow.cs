// <copyright file="BasicWindow.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-6-13  20:50</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shell;

namespace Hummingbird.UI
{
    /// <summary>
    /// Basic Window is classic window which contains a control box with contents. it is used to hosts all other windows except for main window.
    /// </summary>
    /// <remarks>
    /// BasicWindow should be used to all other windows than the main window. You can build the window content the same way as you are building a classic WPF Window. 
    /// The styles, animations will be applied automatically.
    /// </remarks>
    public class BasicWindow : Window, IDisposable
    {

        internal bool useLightIcon = false;

        /// <summary>
        /// Gets the value if the current theme is dark background and white text.
        /// </summary>
        /// <remarks>
        /// In your application, you can use this property to detect the changes of the theme, and adapt for example background image according to the theme applied.
        /// </remarks>
        public bool IsDarkTheme
        {
            get
            {
                Color b = (Color)FindResource("BackgroundColor");
                if ((b.R + b.G + b.B) < 384) return true;
                else return false;
            }
        }

        /// <summary>
        /// Gets or Sets the value to indicate if all components and loading logics has done. 
        /// </summary>
        /// <remarks>
        /// You should manually set HasLoaded = true after initializing all sub contents.
        /// </remarks>
        public bool HasLoaded
        {
            get { return (bool)GetValue(HasLoadedProperty); }
            set { SetValue(HasLoadedProperty, value); }
        }

        /// <summary>
        /// The DependencyProperty for the property HasLoaded
        /// </summary>
        public static readonly DependencyProperty HasLoadedProperty =
            DependencyProperty.Register("HasLoaded", typeof(bool), typeof(BasicWindow));





        /// <summary>
        /// Gets or sets the Sidebar shown on the right side of the window
        /// </summary>
        public Panel SideBar
        {
            get { return (Panel)GetValue(SideBarProperty); }
            set { SetValue(SideBarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SideBar.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The DependencyProperty for SideBar
        /// </summary>
        public static readonly DependencyProperty SideBarProperty =
            DependencyProperty.Register("SideBar", typeof(Panel), typeof(BasicWindow));




        /// <summary>
        /// Gets or sets the Title of the sidebar title.
        /// </summary>
        /// <value>
        /// The side bar title.
        /// </value>
        public string SideBarTitle
        {
            get { return (string)GetValue(SideBarTitleProperty); }
            set { SetValue(SideBarTitleProperty, value); }
        }



        /// <summary>
        /// Gets or sets the maximum width of the <see cref="SideBar"/> panel. the default value is 350 pixels
        /// </summary>
        /// <value>
        /// The maximum width of the side bar.
        /// </value>
        public double SideBarMaxWidth
        {
            get { return (double)GetValue(SideBarMaxWidthProperty); }
            set { SetValue(SideBarMaxWidthProperty, value); }
        }

        /// <summary>
        /// The <see cref="DependencyProperty"/> holding the Maximum width of the SideBar.
        /// </summary>
        public static readonly DependencyProperty SideBarMaxWidthProperty =
            DependencyProperty.Register("SideBarMaxWidth", typeof(double), typeof(BasicWindow), new PropertyMetadata(350d));




        /// <summary>
        /// Gets or sets the minimum width of the <see cref="SideBar"/> panel. the default value is 300 pixels.
        /// </summary>
        /// <value>
        /// The minimum width of the side bar.
        /// </value>
        public double SideBarMinWidth
        {
            get { return (double)GetValue(SideBarMinWidthProperty); }
            set { SetValue(SideBarMinWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SideBarMinWidth.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The <see cref="DependencyProperty"/> holding the Minimum width of the SideBar.
        /// </summary>
        public static readonly DependencyProperty SideBarMinWidthProperty =
            DependencyProperty.Register("SideBarMinWidth", typeof(double), typeof(BasicWindow), new PropertyMetadata(300d));




        // Using a DependencyProperty as the backing store for SideBarTitle.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The DependencyProperty for SideBarTitle
        /// </summary>
        public static readonly DependencyProperty SideBarTitleProperty =
            DependencyProperty.Register("SideBarTitle", typeof(string), typeof(BasicWindow), new PropertyMetadata("Sidebar"));




        /// <summary>
        /// Gets or sets the <see cref="Visibility"/> of The SideBar
        /// </summary>
        /// <value>
        /// The <see cref="Visibility"/> indicates if the <see cref="SideBar"/> is Visible.
        /// </value>
        public Visibility SideBarVisibility
        {
            get { return (Visibility)GetValue(SideBarVisibilityProperty); }
            set { SetValue(SideBarVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SideBarVisibility.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The DependencyProperty for SideBarVisibility
        /// </summary>
        public static readonly DependencyProperty SideBarVisibilityProperty =
            DependencyProperty.Register("SideBarVisibility", typeof(Visibility), typeof(BasicWindow), new PropertyMetadata(Visibility.Collapsed, SideBarVisibilityChanged));

        private static void SideBarVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BasicWindow window && window.Template.FindName("SideBar", window) is ContentControl control)
            {
                double width = control.ActualWidth;
                if (Visibility.Visible == (Visibility)e.NewValue)
                {
                    control.Visibility = Visibility.Visible;
                    if (width == 0) width = 200;
                    var duration = new Duration(new TimeSpan(0, 0, 0, 0, 200));
                    DoubleAnimation da = new DoubleAnimation() { DecelerationRatio = 1, FillBehavior = FillBehavior.HoldEnd, From = width, To = 0, Duration = duration };
                    Storyboard sb = new Storyboard();
                    sb.Children.Add(da);
                    Storyboard.SetTarget(da, control);
                    Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.(TranslateTransform.X)"));
                    control.Visibility = Visibility.Visible;
                    control.BeginStoryboard(sb);
                }
                else
                {
                    var duration = new Duration(new TimeSpan(0, 0, 0, 0, 200));
                    DoubleAnimation da = new DoubleAnimation() { DecelerationRatio = 1, FillBehavior = FillBehavior.HoldEnd, From = 0, To = width, Duration = duration };
                    Storyboard sb = new Storyboard();
                    sb.Children.Add(da);
                    Storyboard.SetTarget(da, control);
                    Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.(TranslateTransform.X)"));
                    sb.Completed += delegate
                    {
                        control.Visibility = Visibility.Collapsed;
                    };
                    control.BeginStoryboard(sb);

                }

            }
        }






        /// <summary>
        /// Gets or sets the identifier of the Window.
        /// </summary>
        /// <value>
        /// The identifier is used to identify the parameters stored (for example: windows size and position or other customizable values).
        /// To properly use these functions, please assign a different Guid in the constructor.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the default style of the Window. 
        /// </summary>
        /// <value>
        /// The default style.
        /// </value>
        protected string DefaultStyle { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="BasicWindow"/> class.
        /// </summary>
        public BasicWindow()
        {
            HasControlBox = true;
            Commands = new ObservableCollection<UIElement>();
            this.WindowStatus = WindowBorderType.Normal;
            this.Initialized += BasicWindow_Initialized;
            this.Loaded += BasicWindow_Loaded;
            this.Closing += BasicWindow_Closing;
            DefaultStyle = "DefaultBasicWindow";
            float dpiX;
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                dpiX = graphics.DpiX;
                if (dpiX == 96)
                {
                    this.UseLayoutRounding = true;
                }
                else
                {
                    this.UseLayoutRounding = false;
                }
            }
        }



        private void BasicWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowManager.Instance.RestoreWindowPosition(this);
        }

        private void BasicWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowManager.Instance.SaveWindowPosition(this);
        }

        /// <summary>
        /// The has control box property, indicate if the windows contains the control box (Minimize, Maximum and Close button)
        /// </summary>
        public static readonly DependencyProperty HasControlBoxProperty = DependencyProperty.Register("HasControlBox", typeof(bool), typeof(BasicWindow));

        /// <summary>
        /// Gets or Sets the value to indicate if current window has Control Box (including Minimize, Maximize and Close button)
        /// </summary>
        public bool HasControlBox
        {
            get
            {
                return (bool)GetValue(HasControlBoxProperty);
            }
            set
            {
                SetValue(HasControlBoxProperty, value);
            }
        }

        /// <summary>
        /// Gets or Sets the value to visually shows the status of the current window, by different border colors.
        /// </summary>
        public WindowBorderType WindowStatus
        {
            get { return (WindowBorderType)GetValue(WindowStatusProperty); }
            set { SetValue(WindowStatusProperty, value); }
        }

        /// <summary>
        /// The Dependency Property for <see cref="WindowStatus"/> property
        /// </summary>
        public static readonly DependencyProperty WindowStatusProperty =
            DependencyProperty.Register("WindowStatus", typeof(WindowBorderType), typeof(BasicWindow));




        /// <summary>
        /// Gets or Sets an collection of UIElements as Commands, these commands will be show on the Title Bar.
        /// </summary>
        /// <remarks>Use only buttons and Menu Items in Commands.</remarks>
        public ObservableCollection<UIElement> Commands
        {
            get
            {
                return (ObservableCollection<UIElement>)GetValue(CommandsProperty);
            }
            set { SetValue(CommandsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Commands.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The commands property
        /// </summary>
        public static readonly DependencyProperty CommandsProperty =
            DependencyProperty.Register("Commands", typeof(ObservableCollection<UIElement>), typeof(BasicWindow), new UIPropertyMetadata());


        /// <summary>
        /// Update the Icon to adapt the changes of theme.
        /// </summary>
        /// <remarks>
        /// This function will be called automatically when initalizing the Windows and applying the theme.
        /// But If the theme is changed by the code after application loads, you must call this function manually.
        /// </remarks>
        public virtual void UpdateIcon()
        {
            Color backgroundColor = (Color)FindResource("BackgroundColor");
            if (backgroundColor.R + backgroundColor.G + backgroundColor.B > 382)
            {
                if (LightIcon != null) Icon = LightIcon;
                else if (DefaultLightIcon != null) Icon = DefaultLightIcon.Clone();
                useLightIcon = true;
            }
            else
            {
                if (DarkIcon != null) Icon = DarkIcon;
                else if (DefaultDarkIcon != null) Icon = DefaultDarkIcon.Clone();
                useLightIcon = false;
            }
        }

        /// <summary>
        /// Updates the background to adapt the changes of theme
        /// </summary>
        /// <remarks>
        /// This function will be called automatically when initalizing the Windows and applying the theme.
        /// But If the theme is changed by the code after application loads, you must call this function manually.
        /// </remarks>
        public void UpdateBackground()
        {
            Color backgroundColor = (Color)FindResource("BackgroundColor");
            if (backgroundColor.R + backgroundColor.G + backgroundColor.B > 382)
            {
                if (LightBackground != null) Background = new ImageBrush(LightBackground) { Stretch = Stretch.UniformToFill };
                else if (DefaultLightBackground != null) Background = new ImageBrush(DefaultLightBackground) { Stretch = Stretch.UniformToFill };
                else { Background = (Brush)FindResource("BackgroundBrush"); }
            }
            else
            {
                if (DarkBackground != null) Background = new ImageBrush(DarkBackground) { Stretch = Stretch.UniformToFill };
                else if (DefaultDarkBackground != null) Background = new ImageBrush(DefaultDarkBackground) { Stretch = Stretch.UniformToFill };
                else { Background = (Brush)FindResource("BackgroundBrush"); }
            }
        }

        /// <summary>
        /// The DependencyProperty for IsBusy property
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(BasicWindow));

        /// <summary>
        /// Gets or Sets the value to indicate if the current window Is Busy.
        /// </summary>
        /// <remarks>
        /// When a window is busy, it will have semi-transparent masks and user can not interact with the UI until the windows turns no busy.
        /// </remarks>
        public bool IsBusy
        {
            get
            {
                return (bool)GetValue(IsBusyProperty);
            }
            set
            {
                SetValue(IsBusyProperty, value);
            }
        }

        private void BasicWindow_Initialized(object sender, EventArgs e)
        {

            this.Style = (Style)FindResource(DefaultStyle);
            Type t = this.Content?.GetType();
            if (t != null)
            {
                var p = t.GetProperty("Background");
                p?.SetValue(this.Content, null);
            }


            UpdateIcon();
            UpdateBackground();

            WindowChrome.SetWindowChrome(this,
                new WindowChrome
                {
                    CaptionHeight = 0,
                    GlassFrameThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(0),
                    ResizeBorderThickness = new Thickness(6),

                });


        }

        /// <summary>
        /// Icon used for Light theme.
        /// </summary>
        /// <remarks>To make the Icon visible in light theme, the icon itself should be Dark enough. </remarks>
        public ImageSource LightIcon { get; set; }

        /// <summary>
        /// Icon used for Dark theme.
        /// </summary>
        /// <remarks>To make the Icon visible in dark theme, the icon itself should be light enough. </remarks>
        public ImageSource DarkIcon { get; set; }

        /// <summary>
        /// Gets or Sets the Default Icon used in Light themes.
        /// </summary>
        /// <remarks>
        /// When This icon is set, it will be applied to all instance of BasicWindow and ModernWindow in your application.
        /// If an BasicWindow and ModernWindow has defined their own LightIcon, then the DefaultLightIcon will not be applied.
        /// If you have multiple Window in the application, set DefaultLightIcon and DefaultDarkIcon in the main window.
        /// </remarks>
        public static ImageSource DefaultLightIcon { get; set; }

        /// <summary>
        /// Gets or Sets the Default Icon used in Dark themes.
        /// </summary>
        /// <remarks>
        /// When This icon is set, it will be applied to all instance of BasicWindow and ModernWindow in your application.
        /// If an BasicWindow and ModernWindow has defined their own DarkIcon, then the DefaultDarkIcon will not be applied.
        /// If you have multiple Window in the application, set DefaultLightIcon and DefaultDarkIcon in the main window.
        /// </remarks>
        public static ImageSource DefaultDarkIcon { get; set; }

        /// <summary>
        /// Gets or Sets the Default background image used in light themes.
        /// </summary>
        /// <remarks>
        /// When This icon is set, it will be applied to all instance of BasicWindow and ModernWindow in your application.
        /// If an BasicWindow and ModernWindow has defined their own LightBackground, then this property will not be used.
        /// </remarks>
        public static ImageSource DefaultLightBackground { get; set; }

        /// <summary>
        /// Gets or Sets the Default background image used in Dark themes.
        /// </summary>
        /// <remarks>
        ///  When This icon is set, it will be applied to all instance of BasicWindow and ModernWindow in your application.
        ///  If an BasicWindow and ModernWindow has defined their own DarkBackground, then this property will not be used.
        /// </remarks>
        public static ImageSource DefaultDarkBackground { get; set; }

        /// <summary>
        /// Background image used for Light theme. the background image itself must be light enough (While).
        /// </summary>
        public ImageSource LightBackground { get; set; }

        /// <summary>
        /// Background image used for Dark theme. the background image itself must be dark enough (Black).
        /// </summary>
        public ImageSource DarkBackground { get; set; }

        /// <summary>
        /// Show a MessageBox
        /// </summary>
        /// <param name="Caption">Title of the MessageBox</param>
        /// <param name="Message">Message</param>
        /// <param name="Buttons">Buttons to show in the MessageBox</param>
        /// <param name="Image">MessageBox Image (not used)</param>
        /// <param name="Details">Detailed error message, can include stack traces and large text to give more detailed information to users.</param>
        /// <returns>The <see cref="MessageBoxResult"/> value according to which button is clicked.</returns>
        public MessageBoxResult ShowMessageBox(string Caption, string Message, MessageBoxButton Buttons = MessageBoxButton.OK, MessageBoxImage Image = MessageBoxImage.None, string Details = null)
        {
            ClassicMessageBox mmb = new ClassicMessageBox(this, Message, Caption, Buttons, Image, Details);

            IsMaskVisible = true;
            var v = mmb.ShowDialog();
            IsMaskVisible = false;
            var result = mmb.Result;
            mmb.Close();
            if (v.HasValue && v.Value)
            {
                return result;
            }
            else
            {
                return MessageBoxResult.None;
            }
        }



        internal bool IsMaskVisible
        {
            get { return (bool)GetValue(IsMaskVisibleProperty); }
            set { SetValue(IsMaskVisibleProperty, value); }
        }

        internal static readonly DependencyProperty IsMaskVisibleProperty =
            DependencyProperty.Register("IsMaskVisible", typeof(bool), typeof(BasicWindow));


        internal bool IsMessageBoxVisible
        {
            get
            {
                return (bool)GetValue(IsMessageBoxVisibleProperty);
            }
            set
            {
                SetValue(IsMessageBoxVisibleProperty, value);
            }
        }
        internal static readonly DependencyProperty IsMessageBoxVisibleProperty = DependencyProperty.Register("IsMessageBoxVisible", typeof(bool), typeof(BasicWindow));

        internal string MessageBoxTitle
        {
            get { return (string)GetValue(MessageBoxTitleProperty); }
            set { SetValue(MessageBoxTitleProperty, value); }
        }
        internal static readonly DependencyProperty MessageBoxTitleProperty = DependencyProperty.Register("MessageBoxTitle", typeof(string), typeof(BasicWindow));

        internal string MessageBoxContent { get { return (string)GetValue(MessageBoxContentProperty); } set { SetValue(MessageBoxContentProperty, value); } }
        internal static readonly DependencyProperty MessageBoxContentProperty = DependencyProperty.Register("MessageBoxContent", typeof(string), typeof(BasicWindow));

        /// <summary>
        /// Gets or Sets a value to indicate where the Title bar is highlighted.
        /// </summary>
        /// <value>
        /// When highlighted, the Titlebar background is shown with HighlightBrush.
        /// </value>
        public bool IsTitleHighlighted
        {
            get { return (bool)GetValue(IsTitleHighlightedProperty); }
            set
            {
                SetValue(IsTitleHighlightedProperty, value);
                if (value)
                {
                    if (DarkIcon != null) Icon = DarkIcon;
                }
                else
                {
                    if (useLightIcon)
                    {
                        if (LightIcon != null) Icon = LightIcon;
                    }
                    else
                    {
                        if (DarkIcon != null) Icon = DarkIcon;
                    }
                }

            }
        }


        /// <summary>
        /// The is title highlighted property
        /// </summary>
        public static readonly DependencyProperty IsTitleHighlightedProperty =
            DependencyProperty.Register("IsTitleHighlighted", typeof(bool), typeof(BasicWindow));


        /// <summary>
        /// Show an In line information in the current window.
        /// </summary>
        /// <param name="title">Title of the information window.</param>
        /// <param name="content">Content of the information window.</param>
        public void ShowInformation(string title, string content)
        {
            MessageBoxTitle = title.ToUpper();
            MessageBoxContent = content;
            IsMaskVisible = true;
            IsMessageBoxVisible = true;
        }

        /// <summary>
        /// Shows the toast notification.
        /// </summary>
        /// <param name="content">The content of the toast notification message.</param>
        /// <param name="level">The notification level.</param>
        /// <param name="callbackAction">This action will be called if the user clicked on the Toast Notification.</param>
        /// <param name="timeout">The timeout in second when the notification hides itself, default value is 0</param>
        /// <remarks>
        /// If multiple toast notification is shown, they will shown one under another.
        /// </remarks>
        public void ShowToastNotification(string content, NotificationLevel level, Action callbackAction = null, int timeout = 5)
        {
            ToastNotificationWindow window = new ToastNotificationWindow(this, content, level, callbackAction, timeout);
            window.Show();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposeValue = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposeValue)
            {
                disposeValue = true;
                if (disposing)
                {
                    if (Content is IDisposable d) d.Dispose();
                    Content = null;
                }

            }
        }
    }
}

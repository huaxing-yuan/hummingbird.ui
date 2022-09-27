// <copyright file="AppbarButton.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-7-9  17:51</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Hummingbird.UI
{
	/// <summary>
    /// AppBarButton are expected to be present in a <see cref="ModernWindow"/> Status bar, displays an vector icon and small text label under it.
    /// According to targeting version of the OS, you can change if a circle is visible outside the icon.
    /// Similar to <see cref="AppBarIcon"/> and <see cref="ModernLink"/>, you specify the icon by a icon key from more than 1200 vector XAML icons or using the Segoe UI Symbol or Segoe UI MDL2 icon set if targeting Windows 8/8.1 or Windows 10
    /// </summary>
	public class AppBarButton : ButtonBase, IDisposable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="AppBarButton"/> class.
        /// </summary>
        public AppBarButton()
		{
            this.Click += AppbarButton_Click;
		}

        private void AppbarButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsToggleButton)
            {
                IsChecked = !IsChecked.HasValue || !IsChecked.Value;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Click -= AppbarButton_Click;
            }
        }


        /// <summary>
        /// The icon key property
        /// </summary>
        public static readonly DependencyProperty IconKeyProperty = DependencyProperty.Register("IconKey", typeof(string), typeof(AppBarButton));


        /// <summary>
        /// Gets or sets the Icon Key of this AppbarButton.
        /// </summary>
        /// <value>
        ///  Hummingbird UI contains more than 1200 modern style icons and you can also use. please refer to Hummingbird UI Icon list to see these icons and keys.
        ///  If you are assigning an unavailable Icon key, you will get a cross icon.
        /// </value>
        public string IconKey{
			get{ return (string)GetValue(IconKeyProperty);}
			set{ SetValue(IconKeyProperty, value);}
		}

        /// <summary>
        /// The caption property
        /// </summary>
        /// <dpdoc />
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(AppBarButton));

        /// <summary>
        /// Gets or sets the caption of the button, shown under the icon with small font.
        /// </summary>
        /// <remarks>
        /// By default, the text is hidden. Only the icon is visible. when the mouse pointer is hover on the button, the text will be shown.
        /// </remarks>
        public string Caption{
			get{ return (string)GetValue(CaptionProperty);}
			set{ SetValue(CaptionProperty, value);}
		}


        /// <summary>
        /// The is toggle button property
        /// </summary>
        public static readonly DependencyProperty IsToggleButtonProperty = DependencyProperty.Register("IsToggleButton", typeof(bool), typeof(AppBarButton));

        /// <summary>
        /// Gets or sets the value if the button is used as a ToggleButton.
        /// </summary>
        /// <value>
        /// True if the AppbarButton is Toggle Button, and the value IsChecked can be used to determine the check status.
        /// False if the AppbarButton is push button
        /// </value>
        public bool IsToggleButton
        {
            get { return (bool)GetValue(IsToggleButtonProperty); }
            set { SetValue(IsToggleButtonProperty, value); }
        }

        /// <summary>
        /// The is checked property
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool?), typeof(AppBarButton));

        /// <summary>
        /// Gets or sets the check status of the button, if the current button is used as toggle button. (IsToggleButton = True)
        /// </summary>
        public bool? IsChecked
        {
            get
            {
                return (bool?)GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }


        /// <summary>
        /// Gets or Sets a value to indicate if a circle is visible outside the icon.
        /// </summary>
        /// <value>
        ///   <c>true</c> to draw a circle outside the icon (Windows Phone and Windows 8.1 style; otherwise, <c>false</c> (Windows 10 style).
        /// </value>

        public bool IsCircleVisible
        {
            get { return (bool)GetValue(IsCircleVisibleProperty); }
            set { SetValue(IsCircleVisibleProperty, value); }
        }


        /// <summary>
        /// The is circle visible property
        /// </summary>
        public static readonly DependencyProperty IsCircleVisibleProperty =
            DependencyProperty.Register("IsCircleVisible", typeof(bool), typeof(AppBarButton), new PropertyMetadata(true));



    }
}

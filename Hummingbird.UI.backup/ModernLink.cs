// <copyright file="ModernLink.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2018-4-19  23:39</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Hummingbird.UI
{
    /// <summary>
    /// ModernLink is a Hyperlink style UI element with Modern Icon
    /// </summary>
    public class ModernLink : ContentControl
    {

        /// <summary>
        /// The icon key property
        /// </summary>
        public static readonly DependencyProperty IconKeyProperty = DependencyProperty.Register("IconKey", typeof(string), typeof(ModernLink));

        /// <summary>
        /// Gets or sets the Icon Key of this ModernLink.
        /// </summary>
        /// <value>
        ///  Hummingbird UI Icon Key
        ///  </value>
        public string IconKey
        {
            get { return (string)GetValue(IconKeyProperty); }
            set { SetValue(IconKeyProperty, value); }
        }




        /// <summary>
        /// Gets or sets the size of the icon appeared in the ModernLink.
        /// </summary>
        /// <value>
        /// The size of the icon, default value is 24;
        /// </value>
        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for IconSize.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(double), typeof(ModernLink), new PropertyMetadata(24d));




        /// <summary>
        /// ForegroundBrush for the icon.
        /// </summary>
        /// <value>
        /// The icon foreground brush.
        /// </value>
        public Brush IconForegroundBrush
        {
            get { return (Brush)GetValue(IconForegroundBrushProperty); }
            set { SetValue(IconForegroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The icon foreground brush property
        /// </summary>
        public static readonly DependencyProperty IconForegroundBrushProperty =
            DependencyProperty.Register("IconForegroundBrush", typeof(Brush), typeof(ModernLink));



        /// <summary>
        /// The text property
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ModernLink));

        /// <summary>
        /// Text of the link
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }




        /// <summary>
        /// Gets or sets the description showing under the Text with smaller font size
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// The <see cref="DependencyProperty"/> of <see cref="Description"/>
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ModernLink));



        /// <summary>
        /// Initializes a new instance of the <see cref="ModernLink"/> class.
        /// </summary>
        public ModernLink()
        {
            this.MouseLeftButtonDown += ModernLink_MouseLeftButtonDown;
            Text = string.Empty;
        }


        /// <summary>
        /// Occurs when user clicks this ModernLink.
        /// </summary>
        public event RoutedEventHandler Click;

        private void ModernLink_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Click?.Invoke(sender, new RoutedEventArgs());
        }
    }

}

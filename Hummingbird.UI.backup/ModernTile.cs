// <copyright file="ModernTile.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2018-4-5  00:12</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Hummingbird.UI
{
    /// <summary>
    /// A Windows 8 start menu style Tile with Icon, Header and background color.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    public class ModernTile : ContentControl
    {


        /// <summary>
        /// Header of the Tile
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(ModernTile));



        /// <summary>
        /// Icon of the Tile
        /// </summary>
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The icon property
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(ModernTile));


        /// <summary>
        /// Initializes a new instance of the <see cref="ModernTile"/> class.
        /// </summary>
        public ModernTile()
        {
            Background = (Brush)FindResource("HighlightBrush");
            Foreground = (Brush)FindResource("HighlightForegroundBrush");
            StartColor = (Color)FindResource("HighlightDarkColor");
            EndColor = (Color)FindResource("HighlightColor");
            Margin = new Thickness(5);
            this.Width = 100;
            this.Height = 100;
        }



        /// <summary>
        /// Gets or sets the corner radius of the current modern tile
        /// </summary>
        /// <value>
        /// The corner radius in <see cref="double"/>.
        /// </value>
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// The <see cref="DependencyProperty"/> of <see cref="CornerRadius"/>
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(ModernTile));




        /// <summary>
        /// Gets or sets the start color for Gradient background.
        /// </summary>
        /// <value>
        /// The start color.
        /// </value>
        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The start color property
        /// </summary>
        public static readonly DependencyProperty StartColorProperty =
            DependencyProperty.Register("StartColor", typeof(Color), typeof(ModernTile));



        /// <summary>
        /// Gets or sets the end color for gradient background
        /// </summary>
        /// <value>
        /// The end color.
        /// </value>
        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The end color property
        /// </summary>
        public static readonly DependencyProperty EndColorProperty =
            DependencyProperty.Register("EndColor", typeof(Color), typeof(ModernTile));




        /// <summary>
        /// Gets or sets a value indicating whether gradient background is used.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the background color is rended with Gradient colors; otherwise, <c>false</c>.
        /// </value>
        public bool UseGradient
        {
            get { return (bool)GetValue(UseGradientProperty); }
            set { SetValue(UseGradientProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UseGradient.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The use gradient property
        /// </summary>
        public static readonly DependencyProperty UseGradientProperty =
            DependencyProperty.Register("UseGradient", typeof(bool), typeof(ModernTile), new PropertyMetadata(false));






    }
}

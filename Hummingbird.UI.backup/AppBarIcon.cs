// <copyright file="AppBarIcon.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-8-31  23:59</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using Hummingbird.UI.Converters;
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
    /// Icon which identified by its IconKey and have circle around it.
    /// </summary>
    /// <remarks>
    /// AppBarIcon shows an vector icon using the same resources from other modern controls like <see cref="ModernLink"/> and <see cref="AppBarButton"/>.
    /// You specify the icon by using the IconKey from more than 1200 samples available or using the icons from Segoe UI Symbol (Windows 8+), or Segoe UI MDL2 (Windows 10).
    /// You can decide if the icon is around by a circle with property <see cref="IsCircleVisible"/>.
    /// AppBarIcon uses Foreground Brush, you can not change the color directly. if you want to change the color of the icon, changes the Foreground of its container.
    /// </remarks>
    public class AppBarIcon : Viewbox
    {
        readonly UIElement Circle = null;
        readonly Grid ContentGrid = new Grid()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        /// <summary>
        /// Initializes a new instance of the <see cref="AppBarIcon"/> class.
        /// </summary>
        public AppBarIcon()
        {
            
            this.Stretch = System.Windows.Media.Stretch.Uniform;
            Grid g = new Grid
            {
                Background = new SolidColorBrush(Colors.Transparent)
            };
            this.Child = g;
            Circle = (UIElement)new StringToResourceConverter().Convert("appbar_base", typeof(UIElement), null, null);
            Circle.Visibility = Visibility.Collapsed;
            g.Children.Add(ContentGrid);
            g.Children.Add(Circle);

        }

        /// <summary>
        /// Gets or sets the Icon Key of this AppbarButton.
        /// </summary>
        /// <value>
        ///  Hummingbird UI contains more than 1000 modern style icons. please refer to Hummingbird UI Icon list to see these icons and keys.
        /// </value>
        public string IconKey
        {
            get { return (string)GetValue(IconKeyProperty); }
            set { SetValue(IconKeyProperty, value); }
        }

        /// <summary>
        /// The DependencyProperty of <see cref="IconKey"/>
        /// </summary>
        public static readonly DependencyProperty IconKeyProperty =
            DependencyProperty.Register("IconKey", typeof(string), typeof(AppBarIcon), new PropertyMetadata(IconChanged));

        private static void IconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue != null)
            {
                UIElement element = (UIElement)new StringToResourceConverter().Convert(e.NewValue, typeof(UIElement), null, null);
                (d as AppBarIcon).ContentGrid.Children.Clear();
                (d as AppBarIcon).ContentGrid.Children.Add(element);

            }
        }

        private static void IsCircleVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarIcon).Circle.Visibility = ((bool)e.NewValue)?Visibility.Visible:Visibility.Collapsed;

        }

        /// <summary>
        /// Gets or Sets a value to indicate if a circle is visible outside the icon.
        /// </summary>
        public bool IsCircleVisible
        {
            get { return (bool)GetValue(IsCircleVisibleProperty); }
            set { SetValue(IsCircleVisibleProperty, value); }
        }

        /// <summary>
        /// The DependencyProperty of <see cref="IsCircleVisible"/>
        /// </summary>
        public static readonly DependencyProperty IsCircleVisibleProperty =
            DependencyProperty.Register("IsCircleVisible", typeof(bool), typeof(AppBarIcon), new PropertyMetadata(IsCircleVisibleChanged));


    }
}

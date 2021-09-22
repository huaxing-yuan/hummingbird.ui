// <copyright file="IconCheckBox.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-10-14  23:39</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Hummingbird.UI
{
    /// <summary>
    /// A Check box with Icon and description
    /// </summary>
    public class IconCheckBox : ButtonBase
    {


        /// <summary>
        /// Gets or Sets a value whether the CheckBox is checked.
        /// </summary>
        public bool? IsChecked
        {
            get { return (bool?)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }


        /// <summary>
        /// The DependencyProperty for IsChecked property
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool?), typeof(IconCheckBox));

        /// <summary>
        /// Gets or Sets the ImageSource of the Icon
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The image source property
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(IconCheckBox));



        /// <summary>
        /// Gets or Sets the header of the IconCheckBox
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(IconCheckBox));



        /// <summary>
        /// Gets or Sets the description of the <see cref="IconCheckBox" />
        /// </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// The DependencyProperty for the Description property
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(IconCheckBox));


        /// <summary>
        /// Initializes a new instance of the <see cref="IconCheckBox"/> class.
        /// </summary>
        public IconCheckBox()
        {
            IsChecked = false;
        }


    }
}

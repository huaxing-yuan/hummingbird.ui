// <copyright file="ModernSwitch.cs" company="Huaxing YUAN">
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

namespace Hummingbird.UI
{
    /// <summary>
    /// A Windows 8 style Switch control. It can replace <see cref="CheckBox"/> in some conditions.
    /// </summary>
    public class ModernSwitch : ContentControl
    {

        /// <summary>
        /// Gets or Sets if the switch is checked.
        /// </summary>
        public bool IsChecked       
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set {
                bool previousValue = (bool)GetValue(IsCheckedProperty);
                if (previousValue != value)
                {
                    SetValue(IsCheckedProperty, value);
                    IsCheckedChanged?.Invoke(this, new RoutedEventArgs());
                }
            }
        }


        /// <summary>
        /// The is checked property
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(ModernSwitch), new PropertyMetadata(false));

        /// <summary>
        /// An event handler invokes when the value of IsChecked property changes
        /// </summary>
        public event RoutedEventHandler IsCheckedChanged;

        /// <summary>
        /// Text to show when the ModernSwitch is checked.
        /// </summary>
        public string CheckedText
        {
            get { return (string)GetValue(CheckedTextProperty); }
            set { SetValue(CheckedTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckedText.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The checked text property
        /// </summary>
        public static readonly DependencyProperty CheckedTextProperty =
            DependencyProperty.Register("CheckedText", typeof(string), typeof(ModernSwitch), new PropertyMetadata("Off"));



        /// <summary>
        /// Text to show when the ModernSwitch is unchecked.
        /// </summary>
        public string UnCheckedText
        {
            get { return (string)GetValue(UnCheckedTextProperty); }
            set { SetValue(UnCheckedTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnCheckedText.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The un checked text property
        /// </summary>
        public static readonly DependencyProperty UnCheckedTextProperty =
            DependencyProperty.Register("UnCheckedText", typeof(string), typeof(ModernSwitch), new PropertyMetadata("On"));

    }
}

// <copyright file="IntToVisibilityConverterReversed.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-9-7  22:59</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Hummingbird.UI.Converters
{
    /// <summary>
    /// Convert <see cref="Int32"/> value to <see cref="Visibility"/>.
    /// </summary>
    /// <remarks>
    /// If the value is 0, the binding element will be <see cref="Visibility.Collapsed"/>, otherwise, the element will be <see cref="Visibility.Visible"/>
    /// </remarks>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class IntToVisibilityConverterReversed : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int v = (int)value;
            return (v == 0) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return 0;
        }
    }
}

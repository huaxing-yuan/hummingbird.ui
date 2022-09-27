// <copyright file="TreeViewLineConverter.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-9-7  23:41</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Hummingbird.UI
{
    class TreeViewLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TreeViewItem item = (TreeViewItem)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}

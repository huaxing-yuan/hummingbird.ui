// <copyright file="SyncTreeViewItem.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-8-2  22:23</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Hummingbird.UI
{
    /// <summary>
    /// Treeview item with "IsSynced" property
    /// </summary>
    /// <seealso cref="System.Windows.Controls.TreeViewItem" />
    public class SyncTreeViewItem : TreeViewItem
    {
        /// <summary>
        /// The is synced property
        /// </summary>
        public static readonly DependencyProperty IsSyncedProperty = DependencyProperty.Register("IsSynced", typeof(bool), typeof(SyncTreeViewItem));
        /// <summary>
        /// Gets or sets a value indicating whether this instance is synced.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is synced; otherwise, <c>false</c>.
        /// </value>
        public bool IsSynced
        {
            get
            {
                return (bool)GetValue(IsSyncedProperty);
            }
            set
            {
                SetValue(IsSyncedProperty, value);
            }
        }
    }
}

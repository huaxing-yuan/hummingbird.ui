// <copyright file="TreeViewItemExtender.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-8-2  22:24</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Hummingbird.UI
{
    /// <summary>
    /// Shows grid lines in a treeview item by using this extender
    /// </summary>
    public class TreeViewItemExtender
    {
        private TreeViewItem _item;

        /// <summary>
        /// The use extender property
        /// </summary>
        public readonly static DependencyProperty UseExtenderProperty = DependencyProperty.RegisterAttached("UseExtender", typeof(bool), typeof(TreeViewItemExtender), new PropertyMetadata(false, OnChangedUseExtender));

        /// <summary>
        /// Gets if the current Extender is used on the given Dependency Object
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns><see langword="true"/> if the extender is used, otherwise <see langword="false"/> </returns>
        public static bool GetUseExtender(DependencyObject sender)
        {
            return (bool)sender.GetValue(UseExtenderProperty);
        }
        /// <summary>
        /// Sets the use extender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="useExtender">if set to <c>true</c> [use extender].</param>
        public static void SetUseExtender(DependencyObject sender, bool useExtender)
        {
            sender.SetValue(UseExtenderProperty, useExtender);
        }

        private static void OnChangedUseExtender(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TreeViewItem item)
            {
                if ((bool)e.NewValue)
                {
                    if (item.ReadLocalValue(ItemExtenderProperty) == DependencyProperty.UnsetValue)
                    {
                        TreeViewItemExtender extender = new TreeViewItemExtender(item);
                        item.SetValue(ItemExtenderProperty, extender);
                    }
                }
                else
                {
                    if (item.ReadLocalValue(ItemExtenderProperty) != DependencyProperty.UnsetValue)
                    {
                        TreeViewItemExtender extender = (TreeViewItemExtender)item.ReadLocalValue(ItemExtenderProperty);
                        extender.Detach();
                        item.SetValue(ItemExtenderProperty, DependencyProperty.UnsetValue);
                    }
                }
            }
        }

        /// <summary>
        /// The item extender property
        /// </summary>
        public static readonly DependencyProperty ItemExtenderProperty = DependencyProperty.RegisterAttached("ItemExtender", typeof(TreeViewItemExtender), typeof(TreeViewItemExtender));

        /// <summary>
        /// The is last one property
        /// </summary>
        public static readonly DependencyProperty IsLastOneProperty = DependencyProperty.RegisterAttached("IsLastOne", typeof(bool), typeof(TreeViewItemExtender));

        /// <summary>
        /// The is toggle button visible property
        /// </summary>
        public static readonly DependencyProperty IsToggleButtonVisibleProperty = DependencyProperty.RegisterAttached("IsToggleButtonVisible", typeof(bool), typeof(TreeViewItemExtender));

        /// <summary>
        /// Gets if the toggle button is visible.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns><see langword="true"/> is the ToggleButton of the TreeViewItem is visible, otherwise <see langword="false"/></returns>
        public static bool GetIsToggleButtonVisible(DependencyObject sender)
        {
            return (bool)sender.GetValue(IsToggleButtonVisibleProperty);
        }


        /// <summary>
        /// Sets if the toggle button visible.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="isToggleButtonVisible">if set to <c>true</c> [is toggle button visible].</param>
        public static void SetIsToggleButtonVisible(DependencyObject sender, bool isToggleButtonVisible)
        {
            sender.SetValue(IsLastOneProperty, isToggleButtonVisible);
        }

        /// <summary>
        /// Gets if the current tree view item is last one in its level.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns><see langword="true"/> is the element is last one in the list, otherwise <see langword="false"/></returns>
        public static bool GetIsLastOne(DependencyObject sender)
        {
            return (bool)sender.GetValue(IsLastOneProperty);
        }
        /// <summary>
        /// Sets if the current tree view item is last one in its level.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="isLastOne">set to <c>true</c> it is the last item in its level</param>
        public static void SetIsLastOne(DependencyObject sender, bool isLastOne)
        {
            sender.SetValue(IsLastOneProperty, isLastOne);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemExtender"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public TreeViewItemExtender(TreeViewItem item)
        {
            _item = item;
            item.Loaded += item_Loaded;
        }

        void item_Loaded(object sender, RoutedEventArgs e)
        {
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(_item);
            if (ic != null)
            {
                ic.ItemContainerGenerator.ItemsChanged += OnItemsChangedItemContainerGenerator;
            
            _item.SetValue(IsLastOneProperty,
                     ic.ItemContainerGenerator.IndexFromContainer(_item) == ic.Items.Count - 1);
            }
            if (_item is SyncTreeViewItem)
            {
                SyncTreeViewItem item = _item as SyncTreeViewItem;
                _item.SetValue(IsToggleButtonVisibleProperty, (!item.IsSynced || item.HasItems));
            }
            else
            {
                _item.SetValue(IsToggleButtonVisibleProperty, _item.HasItems);
            }
        }

        void OnItemsChangedItemContainerGenerator(object sender, ItemsChangedEventArgs e)
        {

            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(_item);

            if (null != ic)
                _item.SetValue(IsLastOneProperty,
                               ic.ItemContainerGenerator.IndexFromContainer(_item) == ic.Items.Count - 1);

            if (_item is SyncTreeViewItem)
            {
                SyncTreeViewItem item = _item as SyncTreeViewItem;
                _item.SetValue(IsToggleButtonVisibleProperty, (!item.IsSynced || item.HasItems));
            }
            else if (_item != null)
            {
                _item.SetValue(IsToggleButtonVisibleProperty, _item.HasItems);
            }
        }

        private void Detach()
        {
            if (_item != null)
            {
                ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(_item);
                ic.ItemContainerGenerator.ItemsChanged -= OnItemsChangedItemContainerGenerator;
                _item.Loaded -= item_Loaded;
                _item = null;
            }
        }
    }
}

// <copyright file="UIHelper.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-10-4  00:07</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Hummingbird.UI
{
    /// <summary>
    /// Helper functions for UI, to get Parents of specified type.
    /// </summary>
    public static class UIHelper
    {

        /// <summary>
        /// Gets a Parent of specific type of an Element
        /// </summary>
        /// <typeparam name="T">Type of the parent to find</typeparam>
        /// <param name="element">The element of which the parent will be found</param>
        /// <returns>The DependencyObject of type T, which is the parent of the UIElement element</returns>
        public static T ParentOfType<T>(DependencyObject element) where T : DependencyObject
        {
            if (element == null)
                return default(T);
            else
                return Enumerable.FirstOrDefault<T>(Enumerable.OfType<T>(GetParents(element)));
        }

        /// <summary>
        /// Gets the parents.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>An Enumerable list containing all visual parents of an element </returns>
        /// <exception cref="ArgumentNullException">element</exception>
        internal static IEnumerable<DependencyObject> GetParents(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            while ((element = GetParent(element)) != null)
                yield return element;
        }

        private static DependencyObject GetParent(DependencyObject element)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            if (parent == null && element is FrameworkElement frameworkElement)
            {
               parent = frameworkElement.Parent;
            }
            return parent;
        }
    }
}

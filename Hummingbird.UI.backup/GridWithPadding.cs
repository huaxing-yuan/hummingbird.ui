// <copyright file="GridWithPadding.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2018-4-5  00:12</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Hummingbird.UI
{
    /// <summary>
    /// A Grid with Padding. Modified from the original article: https://www.codeproject.com/Articles/107468/WPF-Padded-Grid
    /// </summary>
    /// <remarks>
    /// When using GridWithPadding, you can set a global padding value instead of setting the Margin of every children element.
    /// </remarks>
    public class GridWithPadding : Grid
    {
        private static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register("Padding",
                typeof(Thickness), typeof(GridWithPadding),
                new UIPropertyMetadata(new Thickness(0.0),
                new PropertyChangedCallback(OnPaddingChanged)));

        /// <summary>
        /// Padding of all grid children elements.
        /// </summary>
        public Thickness Padding {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridWithPadding"/> class.
        /// </summary>
        public GridWithPadding()
        {
            //  Add a loded event handler.
            Loaded += GridWithMargin_Loaded;
        }


        private static Type[] CenterAlignmentControls =
        {
            typeof(TextBlock),
            typeof(TextBox),
            typeof(Label),
            typeof(Button),
            typeof(RadioButton),
            typeof(CheckBox),
            typeof(ComboBox),
            typeof(IconCheckBox),
            typeof(ModernSwitch),
            typeof(AdornerDecorator),
        };

        void GridWithMargin_Loaded(object sender, RoutedEventArgs e)
        {
            //  Get the number of children.
            int childCount = VisualTreeHelper.GetChildrenCount(this);

            //  Go through the children.
            for (int i = 0; i < childCount; i++)
            {
                //  Get the child.
                DependencyObject child = VisualTreeHelper.GetChild(this, i);
                if (child is AdornerDecorator)
                {

                    UIElement element = (child as AdornerDecorator).Child;
                    if (CenterAlignmentControls.Contains(element.GetType()))
                    {
                        element.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
                    }
                    DependencyProperty marginProperty2 = GetMarginProperty(element);
                    if(marginProperty2 != null)
                    {
                        //  Create the binding.
                        Binding binding = new Binding
                        {
                            Source = this,
                            Path = new PropertyPath("Padding")
                        };

                        //  Bind the child's margin to the grid's padding.
                        BindingOperations.SetBinding(element, marginProperty2, binding);
                    }
                }
                else
                {
                    if (CenterAlignmentControls.Contains(child.GetType()))
                    {
                        VerticalAlignment va = (VerticalAlignment)child.GetValue(VerticalAlignmentProperty);
                        if (va == VerticalAlignment.Stretch)
                        {
                            child.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
                        }
                    }
                    //  Try and get the margin property.
                    DependencyProperty marginProperty = GetMarginProperty(child);
                    //  If we have a margin property, bind it to the padding.
                    if (marginProperty != null)
                    {
                        //  Create the binding.
                        Binding binding = new Binding
                        {
                            Source = this,
                            Path = new PropertyPath("Padding")
                        };

                        //  Bind the child's margin to the grid's padding.
                        BindingOperations.SetBinding(child, marginProperty, binding);
                    }

                }


            }
        }

        /// <summary>
        /// Gets the margin property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>The Margin Property of the given dependency object</returns>
        protected static DependencyProperty GetMarginProperty(DependencyObject dependencyObject)
        {
            //  Go through each property for the object.
            foreach (PropertyDescriptor propertyDescriptor in
                        TypeDescriptor.GetProperties(dependencyObject))
            {
                //  Get the dependency property descriptor.
                DependencyPropertyDescriptor dpd =
                   DependencyPropertyDescriptor.FromProperty(propertyDescriptor);

                //  Have we found the margin?
                if (dpd != null && dpd.Name == "Margin")
                {
                    //  We've found the margin property, return it.
                    return dpd.DependencyProperty;
                }
            }

            //  Failed to find the margin, return null.
            return null;
        }

        private static void OnPaddingChanged(DependencyObject dependencyObject,
               DependencyPropertyChangedEventArgs args)
        {
            //  Get the padded grid that has had its padding changed.
            GridWithPadding paddedGrid = dependencyObject as GridWithPadding;

            //  Force the layout to be updated.
            paddedGrid.UpdateLayout();
        }
    }
}

// <copyright file="ProgressBarSmoother.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2018-5-7  16:16</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Hummingbird.UI
{
    /// <summary>
    /// <para>Shows animation to the ProgressBar when the value has changed</para>
    /// </summary>
    /// <example>
    /// The following example shows how to attach the ProgressBarSmoother to a <see cref="ProgressBar" />, When the Binding object Progress changes, the changes will be shown with animation
    /// <code>
    /// <![CDATA[<ProgressBar Grid.Row="1" Maximum="100" ui:ProgressBarSmoother.Value="{Binding Progress}" Style="{DynamicResource ThinProgressBarNoTrack}"/>]]>
    /// </code>
    /// </example>
    public static class ProgressBarSmoother
    {
        /// <summary>
        /// Gets the smooth value.
        /// </summary>
        /// <param name="obj">The DependencyObject which contains a double value.</param>
        /// <returns>the <see cref="double"/> value within the Dependency Object</returns>
        public static double GetValue(DependencyObject obj)
        {
            var value = double.Parse(obj.GetValue(ValueProperty).ToString());
            if(double.IsNaN(value) || double.IsInfinity(value))
            {
                return 0d;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Sets the smooth value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetValue(DependencyObject obj, double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                //do nothing: do not set value for NaN or infinity.
            }
            else
            {
                obj.SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// The smooth value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(double), typeof(ProgressBarSmoother), new PropertyMetadata(0.0, changing));

        private static void changing(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var anim = new DoubleAnimation((double)e.NewValue, new TimeSpan(0, 0, 0, 0, 500))
            {
                DecelerationRatio = 1
            };
            (d as ProgressBar).BeginAnimation(ProgressBar.ValueProperty, anim, HandoffBehavior.Compose);
        }
    }
}

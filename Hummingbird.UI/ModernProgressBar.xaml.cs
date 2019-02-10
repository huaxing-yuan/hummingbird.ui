// <copyright file="ModernProgressBar.xaml.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-5-5  19:23</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hummingbird.UI
{
    /// <summary>
    /// A Windows 8.1 style progress bar. shows some dots move from left to right. intend to use in application busy mode.
    /// </summary>
    public partial class ModernProgressBar : UserControl
    {
        Storyboard animatingStoryboard1;
        Storyboard animatingStoryboard2;
        Storyboard animatingStoryboard3;
        Storyboard animatingStoryboard4;
        Storyboard animatingStoryboard5;


        /// <summary>
        /// Gets or sets the dot style of the progress bar
        /// </summary>
        /// <value>
        /// The dot style.
        /// </value>
        public DotStyles DotStyle
        {
            get { return (DotStyles)GetValue(DotStyleProperty); }
            set { SetValue(DotStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DotStyle.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The dot style backed DependencyProperty
        /// </summary>
        public static readonly DependencyProperty DotStyleProperty =
            DependencyProperty.Register("DotStyle", typeof(DotStyles), typeof(ModernProgressBar), new PropertyMetadata(DotStyles.Rectangle));




        /// <summary>
        /// Gets or sets the radius of the Circle or Rectangle of the dots.
        /// </summary>
        /// <value>
        /// The radius in integer, default value is 4 point
        /// </value>
        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        /// The backed DependencyProperty for <see cref="Radius"/>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(ModernProgressBar), new PropertyMetadata(4));




        /// <summary>
        /// Initializes a new instance of the <see cref="ModernProgressBar"/> class.
        /// </summary>
        public ModernProgressBar()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            StopAnimation();
            if (Visibility == System.Windows.Visibility.Visible && IsEnabled)
            {
                StartAnimation();
            }
        }

        private void StopAnimation()
        {
            if (animatingStoryboard1 != null)
            {
                if (DotStyle == DotStyles.Rectangle)
                {
                    animatingStoryboard1.Stop(rDot1);
                    animatingStoryboard2.Stop(rDot2);
                    animatingStoryboard3.Stop(rDot3);
                    animatingStoryboard4.Stop(rDot4);
                    animatingStoryboard5.Stop(rDot5);
                }
                else
                {
                    animatingStoryboard1.Stop(rCircle1);
                    animatingStoryboard2.Stop(rCircle2);
                    animatingStoryboard3.Stop(rCircle3);
                    animatingStoryboard4.Stop(rCircle4);
                    animatingStoryboard5.Stop(rCircle5);
                }

                animatingStoryboard1 = null;
                animatingStoryboard2 = null;
                animatingStoryboard3 = null;
                animatingStoryboard4 = null;
                animatingStoryboard5 = null;
            }
        }

        private void StartAnimation()
        {
            StopAnimation();
            double width = this.ActualWidth;
            animatingStoryboard1 = (Storyboard)FindResource("DotStoryboard");
            animatingStoryboard1.Name = "animatingStoryboard1";
            (animatingStoryboard1.Children.ElementAt(0) as DoubleAnimation).To = width * 250 / 600;
            (animatingStoryboard1.Children.ElementAt(1) as DoubleAnimation).To = width * 350 / 600;
            (animatingStoryboard1.Children.ElementAt(2) as DoubleAnimation).To = width;
            animatingStoryboard2 = animatingStoryboard1.Clone();
            animatingStoryboard2.BeginTime = new TimeSpan(0, 0, 0, 0, 150);
            animatingStoryboard3 = animatingStoryboard1.Clone();
            animatingStoryboard3.BeginTime = new TimeSpan(0, 0, 0, 0, 300);
            animatingStoryboard4 = animatingStoryboard1.Clone();
            animatingStoryboard4.BeginTime = new TimeSpan(0, 0, 0, 0, 450);
            animatingStoryboard5 = animatingStoryboard1.Clone();
            animatingStoryboard5.BeginTime = new TimeSpan(0, 0, 0, 0, 600);

            if (DotStyle == DotStyles.Rectangle)
            {
                animatingStoryboard1.Begin(rDot1, true);
                animatingStoryboard2.Begin(rDot2, true);
                animatingStoryboard3.Begin(rDot3, true);
                animatingStoryboard4.Begin(rDot4, true);
                animatingStoryboard5.Begin(rDot5, true);
            }
            else
            {
                animatingStoryboard1.Begin(rCircle1, true);
                animatingStoryboard2.Begin(rCircle2, true);
                animatingStoryboard3.Begin(rCircle3, true);
                animatingStoryboard4.Begin(rCircle4, true);
                animatingStoryboard5.Begin(rCircle5, true);

            }

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility != System.Windows.Visibility.Visible)
            {
                StopAnimation();
            }
            else
            {
                if (this.IsEnabled)
                {
                    StartAnimation();
                }
                else
                {
                    StopAnimation();
                }
            }
        }

        private void UserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled)
            {
                StartAnimation();
            }
            else
            {
                StopAnimation();
            }
        }
    }
}


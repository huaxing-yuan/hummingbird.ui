// <copyright file="ModernCounter.xaml.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-5-22  19:15</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using Hummingbird.UI.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Modern Counter is a little history report UI elements.
    /// </summary>
    public partial class ModernCounter : Border
    {
        private Brush Brush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        private Brush FillBrush = new SolidColorBrush(Color.FromArgb(60, 0, 0, 0));



        private Color _color;

        /// <summary>
        /// Gets or Sets the Color of the text and Graph, with animation when the value has changed.
        /// </summary>
        public Color TextColor
        {
            get { return _color; }
            set {
                _color = value;
                Brush = new SolidColorBrush(value);
                Color c = Color.FromArgb(60, value.R, value.G, value.B);
                FillBrush = new SolidColorBrush(c);
                txtHeader.Foreground = Brush;
                txtNumber.Foreground = Brush;
            }
        }

        private int _displayNumber;


        /// <summary>
        /// Gets an integer value as the counter (it sums up all values in the Data)
        /// </summary>
        public int DisplayNumber
        {
            get { return _displayNumber;  }
            private set {
                if (Math.Abs(value - _displayNumber) <= 3)
                {
                    _displayNumber = value;
                }
                else
                {
                    RunAnimation(value);
                    _displayNumber = value;
                }
                txtNumber.Text = value.ToString();
            }
        }


        private void RunAnimation(int value)
        {
            var timeline = new StringAnimationUsingKeyFrames();
            long tick = 200000;
            if (value > _displayNumber)
            {
                double jump = Math.Abs(value - _displayNumber) / 100d;
                for (double i = _displayNumber; i < value; i += jump)
                {
                    string stringvalue = ((int)i).ToString();
                    timeline.KeyFrames.Add(new DiscreteStringKeyFrame(stringvalue, KeyTime.FromTimeSpan(new TimeSpan(tick))));
                    tick += 200000;
                }
            }
            else
            {
                double jump = Math.Abs(value - _displayNumber) / 100d;
                for (double i = _displayNumber; i > value; i -= jump)
                {
                    string stringvalue = ((int)i).ToString();
                    timeline.KeyFrames.Add(new DiscreteStringKeyFrame(stringvalue, KeyTime.FromTimeSpan(new TimeSpan(tick))));
                    tick += 200000;
                }
            }
            timeline.KeyFrames.Add(new DiscreteStringKeyFrame(value.ToString(), KeyTime.FromTimeSpan(new TimeSpan(tick))));
            timeline.DecelerationRatio = 1;
            timeline.FillBehavior = FillBehavior.HoldEnd;
            txtNumber.BeginAnimation(TextBlock.TextProperty, timeline);
        }

        int[] _data;


        /// <summary>
        /// Gets or Sets an Integer Array to holds the history. When Data is Set DisplayNumber will be updated as the sum of the integer values
        /// </summary>
        [TypeConverter(typeof(StringToIntArrayTypeConverter))]
        public int[] Data
        {
            get { return _data; }
            set {
                _data = value;
                if (IsLoaded)
                {
                    DrawBars();
                }
                DisplayNumber = GetSum(value);
            }
        }
        private static int GetSum(int[] p)
        {
            int total = 0;
            foreach (var i in p)
            {
                total += i;
            }
            return total;
        }

        private void DrawBars()
        {
            try
            {
                if (_data == null || _data.Length <= 1)
                {
                    return;
                }
                else
                {
                    double realWidth = cDrawer.ActualWidth / (_data.Length - 1);

                    //find the maximum one:
                    int max = 0;
                    foreach (int data in _data)
                    {
                        if (data > max)
                        {
                            max = data;
                        }
                    }
                    Polygon polygon = new Polygon
                    {
                        SnapsToDevicePixels = true
                    };
                    polygon.Points.Add(new Point(0, cDrawer.ActualHeight));
                    for (int i = 0; i < _data.Length; i++)
                    {
                        double height = (max == 0 ? 0 : cDrawer.ActualHeight * _data[i] / max);
                        double x = i * realWidth;
                        double y = cDrawer.ActualHeight - height;
                        polygon.Points.Add(new Point(x, y));
                        Rectangle r = new Rectangle() { Width = realWidth, Height = height, Fill = new SolidColorBrush(Colors.Transparent) };
                        Panel.SetZIndex(r, 100);
                        r.ToolTip = _data[i];
                        Canvas.SetTop(r, y - 2);
                        Canvas.SetLeft(r, x - 2);
                        cDrawer.Children.Add(r);
                    }
                    polygon.Points.Add(new Point((_data.Length - 1) * realWidth, cDrawer.ActualHeight));
                    polygon.Stroke = Brush;
                    polygon.Fill = FillBrush;
                    polygon.Style = (Style)FindResource("AnimatedPolygon");
                    cDrawer.Children.Clear();
                    cDrawer.Children.Add(polygon);
                }
            }
            catch
            {
                //nothing to do here
            }
        }



        /// <summary>
        /// Gets or Sets the header of the ModernCounter
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
            DependencyProperty.Register("Header", typeof(string), typeof(ModernCounter));




        /// <summary>
        /// Initializes a new instance of the <see cref="ModernCounter"/> class.
        /// </summary>
        public ModernCounter()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        private void cDrawer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawBars();
        }

    }

}

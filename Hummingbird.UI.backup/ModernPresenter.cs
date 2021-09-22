// <copyright file="ModernPresenter.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-10-19  23:09</date>
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
    /// A container control which holds others controls panning horizontally.
    /// </summary>
    public class ModernPresenter : ScrollViewer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModernPresenter"/> class.
        /// </summary>
        public ModernPresenter()
        {
            this.PreviewMouseWheel += ModernPresenter_MouseWheel;
            this.MouseEnter += ModernPresenter_MouseEnter;
            this.MouseLeave += ModernPresenter_MouseLeave;
            VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            this.PanningMode = System.Windows.Controls.PanningMode.HorizontalFirst;
        }



        /// <summary>
        /// Gets or sets a value indicating whether this to hide the Scroll Bar even on mouse over.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this always hide Scroll Bar; otherwise, <c>false</c>.
        /// </value>
        public bool IsScrollBarHidden
        {
            get { return (bool)GetValue(IsScrollBarHiddenProperty); }
            set { SetValue(IsScrollBarHiddenProperty, value); }
        }


        /// <summary>
        /// The DependencyProperty declaration for <see cref="IsScrollBarHidden"/>
        /// </summary>
        public static readonly DependencyProperty IsScrollBarHiddenProperty =
            DependencyProperty.Register("IsScrollBarHidden", typeof(bool), typeof(ModernPresenter), new PropertyMetadata(false));



        private void ModernPresenter_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }


        void ModernPresenter_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsScrollBarHidden) return;
            if (!this.IsFocused)
            {
                this.Focus();
            }
            this.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        void ModernPresenter_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (this.HasContent)
            {
                int delta = e.Delta;
                if (e.Delta > 0)
                {
                    while (delta > 0)
                    {
                        this.LineLeft();
                        delta -= 20;
                    }
                }
                else
                {
                    while (delta < 0)
                    {
                        this.LineRight();
                        delta += 20;
                    }
                }
            }
        }
    }
}

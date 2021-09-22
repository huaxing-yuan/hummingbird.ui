// <copyright file="DetachedWindowHost.xaml.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-5-22  23:12</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hummingbird.UI
{
    /// <summary>
    /// This is a Window Host for all detached contents
    /// </summary>
    internal partial class DetachedWindowHost : BasicWindow
    {
        EventHandler windowCloseEvent;
        ModernContent ModernContent;
        public DetachedWindowHost(string Title, ModernContent modernContent, EventHandler WindowClosedEvent)
        {
            ModernContent = modernContent;
            InitializeComponent();
            windowCloseEvent = WindowClosedEvent;
            this.Title = Title??"Untitled";
        }


        private void BasicWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.BorderCommandArea.Child = null;
            this.BorderContent.Child = null;
            this.BorderStatusBar.Child = null;
            windowCloseEvent?.Invoke(this, EventArgs.Empty);
        }



        private void BasicWindow_Initialized(object sender, EventArgs e)
        {
            this.BorderCommandArea.Child = ModernContent.CommandArea;
            this.BorderContent.Child = ModernContent;
            this.BorderStatusBar.Child = ModernContent.StatusBar;

        }
    }
}

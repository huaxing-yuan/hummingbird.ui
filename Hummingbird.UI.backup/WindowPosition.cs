// <copyright file="WindowPosition.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-10-12  21:07</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hummingbird.UI
{
    [Serializable]
    internal class WindowPosition
    {
        internal WindowState WindowState { get; set; }
        internal double Left { get; set; }
        internal double Top { get; set; }
        internal double Width { get; set; }
        internal double Height { get; set; }

    }
}

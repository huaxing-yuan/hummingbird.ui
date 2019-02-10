// <copyright file="WindowBorderType.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-10-8  22:43</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hummingbird.UI
{
    /// <summary>
    /// Type of the WindowBorder
    /// </summary>
    public enum WindowBorderType
    {
        /// <summary>
        /// Window is in Normal state
        /// </summary>
        Normal,
        /// <summary>
        /// Window is in Busy State
        /// </summary>
        Busy,
        /// <summary>
        /// Window is in Good state, a Green style border will be shown
        /// </summary>
        Good,
        /// <summary>
        /// Window is in Bad state, a Red border will be shown
        /// </summary>
        Bad,
    }
}

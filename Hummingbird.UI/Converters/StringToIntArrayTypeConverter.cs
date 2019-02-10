// <copyright file="StringToIntArrayTypeConverter.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-9-7  22:59</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hummingbird.UI.Converters
{
    /// <summary>
    /// A converter converts from <see cref="String"/> to Array of <see cref="Int32"/>
    /// </summary>
    /// <remarks>
    /// The input string must in form of integer numbers separated by comma (,)
    /// </remarks>
    public class StringToIntArrayTypeConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if(sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            try
            {
                if (value is string && value != null)
                {
                    string[] vs = ((string)value).Split(',');
                    List<int> values = new List<int>();
                    foreach (var v in vs)
                    {
                        values.Add(int.Parse(v));
                    }
                    return values.ToArray();
                }
                return base.ConvertFrom(context, culture, value);
            }
            catch
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if(value is int[])
            {
                return string.Join<int>(",", (int[])value);
            }
            return base.ConvertTo(value, destinationType);
        }
    }
}

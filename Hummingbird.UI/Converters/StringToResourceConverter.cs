// <copyright file="StringToResourceConverter.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-9-7  22:59</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Hummingbird.UI.Converters
{

    /// <summary>
    /// This converter is used to convert a <see cref="String"/> value to an UI Element. espcially used in <see cref="AppBarButton.IconKey"/> Property.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    /// <remarks>
    /// The accept string are:
    /// <list type="bullet">
    /// <item>"appbar_*" for built-in XAML vector icons, usable for all version of Windows.</item>
    /// <item>"symbol_*" for Icons defined in Segoe UI Symbol font, avaiable in Windows 8, Windows 8.1 and Windows 10</item>
    /// <item>"mdl2_*" for Icons defined in Segoe UI MDL2 Asset font, available in Windows 10</item>
    /// </list>
    /// </remarks>
    public class StringToResourceConverter : IValueConverter
    {

        private static FontFamily SymbolFontFamily = new FontFamily("Segoe UI Symbol");
        private static FontFamily Mdl2FontFamily = new FontFamily("Segoe MDL2 Assets");
        #region IValueConverter implementation

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string stringValue = value?.ToString();
            if (stringValue == null) return null;
            object p = Application.Current.TryFindResource(stringValue);
            if (p is UIElement)
            {
                int i = 1;
                if (!(Application.Current.TryFindResource(stringValue + "__" + i) is UIElement child))
                {
                    var xaml = System.Windows.Markup.XamlWriter.Save(p);
                    var deepCopy = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
                    return deepCopy;
                }
                else
                {
                    Canvas c = new Canvas() { Height = 76, Width = 76 };
                    c.Children.Add(p as UIElement);
                    while (child != null)
                    {
                        c.Children.Add(child);
                        i++;
                        child = Application.Current.TryFindResource(stringValue + "__" + i) as UIElement;
                    }
                    var xaml = System.Windows.Markup.XamlWriter.Save(c);
                    var deepCopy = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
                    c.Children.Clear();
                    return deepCopy;
                }
            }
            else if(p is string)
            {
                if (stringValue.StartsWith("symbol_"))
                {
                    TextBlock tb = new TextBlock()
                    {
                        Text = p as string,
                        FontSize = 38,
                        FontFamily = SymbolFontFamily,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    return tb;
                }
                else if(stringValue.StartsWith("mdl2_"))
                {
                    TextBlock tb = new TextBlock()
                    {
                        Text = p as string,
                        FontSize = 42,
                        FontFamily = Mdl2FontFamily,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    return tb;
                }
                else
                {
                    UIElement empty = Application.Current.FindResource("appbar_close") as UIElement;
                    var xaml = System.Windows.Markup.XamlWriter.Save(empty);
                    var deepCopy = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
                    return deepCopy;
                }
            }
            else
            {
                UIElement empty = Application.Current.FindResource("appbar_close") as UIElement;
                var xaml = System.Windows.Markup.XamlWriter.Save(empty);
                var deepCopy = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
                return deepCopy;
            }
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}

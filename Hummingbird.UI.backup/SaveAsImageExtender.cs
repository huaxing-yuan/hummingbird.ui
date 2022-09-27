using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hummingbird.UI
{
    /// <summary>
    /// <see cref="SaveAsImageExtender"/> gives the ability to save any content of a <see cref="FrameworkElement"/> as image. 
    /// </summary>
    /// <remarks>
    /// When this extender is attached with <see cref="IsActiveProperty"/> = <see langword="true"/>, user can right click the element to export it's content to clipboard as Image.
    /// </remarks>
    public static class SaveAsImageExtender 
    {



        /// <summary>
        /// Gets if the current Extender is used on the given Dependency Object
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns>
        ///   <see langword="true" /> if the extender is used, otherwise <see langword="false" />
        /// </returns>
        public static bool GetIsActive(DependencyObject sender)
        {
            return (bool)sender.GetValue(IsActiveProperty);
        }
        /// <summary>
        /// Sets the use extender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="isActive">if set to <c>true</c> [use extender].</param>
        public static void SetIsActive(DependencyObject sender, bool isActive)
        {
            sender.SetValue(IsActiveProperty, isActive);
        }

        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The is active property
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(SaveAsImageExtender), new PropertyMetadata(false, OnIsActiveChanged));

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement fe)
            {
                if ((bool)e.NewValue)
                {
                    //intercept mouserightbuttondown
                    if (fe.ReadLocalValue(SaveAsImageExtenderProperty) == DependencyProperty.UnsetValue)
                    {
                        fe.PreviewMouseRightButtonDown += Fe_PreviewMouseRightButtonDown;
                    }
                }
                else
                {
                    if (fe.ReadLocalValue(SaveAsImageExtenderProperty) != DependencyProperty.UnsetValue)
                    {
                        fe.PreviewMouseRightButtonDown -= Fe_PreviewMouseRightButtonDown;
                    }
                }
            }
        }

        private static void Fe_PreviewMouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe)
            {
                var bitmap = ConvertToBitmap(fe);
                Clipboard.SetImage(bitmap);
                e.Handled = false;
                var parent = UIHelper.ParentOfType<BasicWindow>(fe);
                parent?.ShowToastNotification("This element is copied to clipboard as image", NotificationLevel.Information);
            }
        }

        private static RenderTargetBitmap ConvertToBitmap(FrameworkElement uiElement)
        {

            var bmp = new RenderTargetBitmap((int)uiElement.ActualWidth, (int)uiElement.ActualHeight, 96, 96, PixelFormats.Default);
            bmp.Render(uiElement);

            return bmp;
        }

        /// <summary>
        /// The item extender property
        /// </summary>
        public static readonly DependencyProperty SaveAsImageExtenderProperty = DependencyProperty.RegisterAttached("SaveAsImageExtender", typeof(SaveAsImageExtender), typeof(SaveAsImageExtender));

    }
}

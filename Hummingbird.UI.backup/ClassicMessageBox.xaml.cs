// <copyright file="ClassicMessageBox.xaml.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-10-17  00:34</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Hummingbird.UI
{
    /// <summary>
    /// Interaction logic for ModernMessageBox.xaml
    /// </summary>
    public partial class ClassicMessageBox : BasicWindow
    {
        internal MessageBoxResult Result { get; private set; }

        internal ClassicMessageBox(Window ParentWindow, string Content, string Caption, MessageBoxButton Buttons, MessageBoxImage Image, string DetailedError = null)
        {
            if (ParentWindow != null)
            {
                this.Title = ParentWindow.Title;
                this.Icon = ParentWindow.Icon;
                this.Owner = ParentWindow;
            }
            else
            {
                this.Title = "Hummingbird";
            }
            InitializeComponent();
            if (Caption != null)
            {
                txtHeader.Text = Caption.ToUpper();
            }
            else
            {
                if (ParentWindow != null)
                {
                    txtHeader.Text = ParentWindow.Title.ToUpper();
                }
                else
                {
                    txtHeader.Text = this.Title.ToUpper();
                }
            }

            txtMessage.Text = Content;
            if (!string.IsNullOrEmpty(DetailedError))
            {
                txtDetails.Tag = DetailedError;
                txtShowDetails.Tag = DetailedError;
                spDetails.Visibility = Visibility.Visible;
            }
            else
            {
                spDetails.Visibility = Visibility.Collapsed;
            }
            switch (Buttons)
            {
                case MessageBoxButton.OK:
                    BuildButton(MessageBoxResult.OK);
                    break;
                case MessageBoxButton.OKCancel:
                    BuildButton(MessageBoxResult.OK);
                    BuildButton(MessageBoxResult.Cancel);
                    break;
                case MessageBoxButton.YesNo:
                    BuildButton(MessageBoxResult.Yes);
                    BuildButton(MessageBoxResult.No);
                    break;
                case MessageBoxButton.YesNoCancel:
                    BuildButton(MessageBoxResult.Yes);
                    BuildButton(MessageBoxResult.No);
                    BuildButton(MessageBoxResult.Cancel);
                    break;
            }
        }

        private void BuildButton(MessageBoxResult Value)
        {
            Style buttonstyle = (Style)FindResource("DialogButton");
            Button button = new Button()
            {
                Content = Value.ToString().ToUpper(),
                Tag = Value,
                Style = buttonstyle,
            };
            if (Value == MessageBoxResult.OK || Value == MessageBoxResult.Yes)
            {
                button.IsDefault = true;
            }
            if (Value == MessageBoxResult.Cancel)
            {
                button.IsCancel = true;
            }
            button.Click += new RoutedEventHandler(ButtonClick);
            spButtons.Children.Add(button);
            
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Result = (MessageBoxResult)button.Tag;
            try
            {
                DialogResult = true;
            }
            catch {
                //nothing to do here
            }
            Close();
        }

        private static Window GetParentWindow(DependencyObject child)
        {
            if (child == null)
            {
                return null;
            }
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }

            if (parentObject is Window parent)
            {
                return parent;
            }
            else
            {
                return GetParentWindow(parentObject);
            }
        }

        /// <summary>
        /// Shows the message box.
        /// </summary>
        /// <param name="child">The component calling the message box.</param>
        /// <param name="Caption">The caption (header).</param>
        /// <param name="Message">The message (content)</param>
        /// <param name="Buttons">The buttons of message box</param>
        /// <param name="Image">The icon indicating the level of the message (this parameter is not yet used in the current version of UI Framework).</param>
        /// <param name="DetailedError">The detailed error.</param>
        /// <returns>The <see cref="MessageBoxResult"/> value indication the choose of the user.</returns>
        public static MessageBoxResult ShowMessageBox(DependencyObject child, string Caption, string Message, MessageBoxButton Buttons = MessageBoxButton.OK, MessageBoxImage Image = MessageBoxImage.Information, string DetailedError = null)
        {
            Window w = GetParentWindow(child);
            if (w is BasicWindow bw)
            {
                return bw.ShowMessageBox(Caption, Message, Buttons, Image, DetailedError);
            }
            else if (w != null)
            {
                ClassicMessageBox cmb = new ClassicMessageBox(w, Message, Caption, Buttons, Image, DetailedError)
                {
                    Owner = w
                };
                var v = cmb.ShowDialog();
                if (v.HasValue && v.Value)
                {
                    var result = cmb.Result;
                    return result;
                }
                return MessageBoxResult.OK;
            }
            else
            {
                ClassicMessageBox cmb = new ClassicMessageBox(null, Message, Caption, Buttons, Image, DetailedError);
                var v =cmb.ShowDialog();
                if (v.HasValue && v.Value)
                {
                    var result = cmb.Result;
                    return result;
                }
                return MessageBoxResult.OK;
            }
        }

        private void txtDetails_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string details = (sender as TextBlock).Tag.ToString();
            Clipboard.SetText(details);
            txtDetails.Text = "Detailed information has been copied to clipboard.";
        }

        private void txtShowDetails_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string details = (sender as TextBlock).Tag.ToString();
            txtDetailedMessages.Visibility = Visibility.Visible;
            txtDetailedMessages.Text = details;
            sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        }
    }
}

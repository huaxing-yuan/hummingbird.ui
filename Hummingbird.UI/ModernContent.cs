// <copyright file="ModernContent.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2018-4-5  00:12</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Linq;

namespace Hummingbird.UI
{
    /// <summary>
    /// Custom UserControl used in BasicWindow or ModernWindow
    /// </summary>
    public class ModernContent : ContentControl, IDisposable
    {
        /// <summary>
        /// Unique identifier of this ModernContent, it is usually used to save personal data per control
        /// </summary>
        public Guid StrongName { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ModernContent"/> class.
        /// </summary>
        public ModernContent()
        {
            this.RenderTransform = new TranslateTransform();
        }

        /// <summary>
        /// Gets the value if the current theme is dark background and white text.
        /// </summary>
        /// <remarks>
        /// In your application, you can use this property to detect the changes of the theme, and adapt for example background image according to the theme applied.
        /// </remarks>
        public bool IsDarkTheme
        {
            get
            {
                Color b = (Color)FindResource("BackgroundColor");
                if ((b.R + b.G + b.B) < 384) return true;
                else return false;
            }
        }

        /// <summary>
        /// Gets or Sets a value indicates if the Load operation is done for this control. you need to pass manually this indicator to True once initialization has done.
        /// </summary>
        protected bool HasLoaded { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernContent"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public ModernContent(ModernWindow parent)
        {
            this.RenderTransform = new TranslateTransform();
        }

        /// <summary>
        /// Gets or Sets the value of Window name showing in Title bar, when the current ModernContent is detached.
        /// </summary>
        public string DetachedName
        {
            get { return ( string)GetValue(DetachedNameProperty); }
            protected set { SetValue(DetachedNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DetachedName.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The Dependency property for DetachedName
        /// </summary>
        public static readonly DependencyProperty DetachedNameProperty =
            DependencyProperty.Register("DetachedName", typeof( string), typeof(ModernContent));




        /// <summary>
        /// Calls before closing the window. By default this function triggers Closing event. if the event is canceled, the result is false.
        /// If the Closing event is not defined, then True will be returned.
        /// This function can be overridden, to prompt user to save changes before content closing.
        /// </summary>
        /// <returns>True indicating that the content can be closed, or False that the content cannot be closed.</returns>
        public virtual bool CanBeClosed()
        {
            if (Closing != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                Closing(this, args);
                if (args.Cancel)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }



        /// <summary>
        /// When the content will be replaced in a ModernWindow, this event will be fired in order to save the changes.
        /// </summary>
        /// <remarks>
        /// Assign <see cref="CancelEventArgs.Cancel"/> = True to cancel the closing operation. 
        /// </remarks>
        public event CancelEventHandler Closing;

        /// <summary>
        /// The BasicWindow object who holds the current ModernContent
        /// </summary>
        /// <remarks>
        /// <para>If the ModernContent does not held by a BasicWindow or ModernWindow, null value will be returned.</para>
        /// <para>The method <see cref="ShowInformation(string, string)"/>, <see cref="ShowMessageBox(string, string, MessageBoxButton, MessageBoxImage, string)"/>, <see cref="ShowToastNotification(string, NotificationLevel, Action, int)"/> uses this property
        /// to show information in the right position. if the current ModernContent does not hold by a <see cref="BasicWindow"/> or <see cref="ModernWindow"/> these methods fails.</para>
        /// </remarks>
        public BasicWindow ParentWindow
        {
            get
            {
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow is BasicWindow bw) return bw;
                else if (parentWindow is ModernWindow mw) return mw;
                else return null;
            }
        }

        /// <summary>
        /// Show a MessageBox
        /// </summary>
        /// <param name="Title">Title of the MessageBox</param>
        /// <param name="Message">Message</param>
        /// <param name="Buttons">Buttons to show in the MessageBox</param>
        /// <param name="Image">MessageBox Image (not used)</param>
        /// <param name="Details">Detailed error message, can include stack traces and large text</param>
        /// <returns>The <see cref="MessageBoxResult"/> value according to which button is clicked.</returns>
        public MessageBoxResult ShowMessageBox(string Title, string Message, MessageBoxButton Buttons = MessageBoxButton.OK, MessageBoxImage Image = MessageBoxImage.None, string Details = null)
        {
            return ParentWindow.ShowMessageBox(Title, Message, Buttons, Image, Details);
        }

        /// <summary>
        /// Shows the message box with detailed error message.
        /// </summary>
        /// <param name="Title">Title of the MessageBox.</param>
        /// <param name="Message">Content of the message.</param>
        /// <param name="Details">The details message (can contains detailed error messages)</param>
        /// <returns>The <see cref="MessageBoxResult"/> value according to which button is clicked.</returns>
        public MessageBoxResult ShowMessageBox(string Title, string Message, string Details)
        {
            return ParentWindow.ShowMessageBox(Title, Message, MessageBoxButton.OK, MessageBoxImage.Error, Details);
        }

        /// <summary>
        /// Show an Windows 8.1 style information panel in the current window.
        /// </summary>
        /// <param name="title">Title of the information window.</param>
        /// <param name="content">Content of the information window.</param>
        public void ShowInformation(string title, string content)
        {
            ParentWindow.ShowInformation(title, content);
        }

        /// <summary>
        /// Shows a toast notification of given Content and Level.
        /// </summary>
        /// <param name="content">The content of the toast notification message.</param>
        /// <param name="level">The notification level.</param>
        /// <param name="callbackAction">This action will be called if the user clicked on the Toast Notification.</param>
        /// <param name="timeout">The timeout in second when the notification hides itself, default value is 5</param>
        public void ShowToastNotification(string content, NotificationLevel level, Action callbackAction = null, int timeout = 5)
        {
            ParentWindow.ShowToastNotification(content, level, callbackAction, timeout);
        }



        /// <summary>
        /// The is detachable property
        /// </summary>
        public static readonly DependencyProperty IsDetachableProperty = DependencyProperty.Register("IsDetachable", typeof(bool), typeof(ModernContent));

        /// <summary>
        /// Gets or Sets a value indicates if the current content can be detached.
        /// </summary>
        /// <remarks>
        /// When the content can be detached, an icon will be shown in your ModernWindow.
        /// When detached, the content will be shown in an independent window, This functionality is useful when you have multi-screen.
        /// </remarks>
        public bool IsDetachable
        {
            get
            {
                return (bool)GetValue(IsDetachableProperty);
            }
            set
            {
                SetValue(IsDetachableProperty, value);
            }
        }

        /// <summary>
        /// Gets or Sets the statusBar of the ModernContent. 
        /// </summary>
        /// <remarks>
        /// This status bar will replace the default status bar of the ModernWindow if the current ModernContent is the active content.
        /// If you do not have specific status bar items, assign this property to NULL, the default status bar will be remained.
        /// StatusBar is used only in ModernWindow, if you have a ModernContent in BasicWindow, this StatusBar will not be shown.
        /// </remarks>
        public StatusBar StatusBar { get; set; }

        /// <summary>
        /// Gets or Sets the CommandArea of the ModernContent. 
        /// </summary>
        /// <remarks>
        /// Command Area is a little space to holds some controls on the right side of the ModernWindow, at the same level of MainMenu.
        /// CommandArea is used only in ModernWindow
        /// </remarks>
        public StackPanel CommandArea { get; set; }


        /// <summary>
        /// Determines whether this content contains validation errors of binding.
        /// </summary>
        /// <param name="obj">The Root dependency object.</param>
        /// <returns>
        ///   <c>true</c> if at least one controls are errors; otherwise, <c>false</c>.
        /// </returns>
        private bool HasNoValidationError(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors and all
            // of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&
            LogicalTreeHelper.GetChildren(obj)
            .OfType<DependencyObject>()
            .All(HasNoValidationError);
        }

        /// <summary>
        /// Determines whether this content contains validation errors of Data Binding.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if at least one controls are errors; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// You can check the value before saving, closing to ensure all user input is correct.
        /// </remarks>
        protected bool HasNoValidationError()
        {
            return HasNoValidationError(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Closing = null;
                if (Content is IDisposable d) d.Dispose();
                Content = null;
            }
        }




        /// <summary>
        /// Gets or sets a value indicating whether the data held in this ModernContent has been changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the content is changed; otherwise, <c>false</c>.
        /// </value>
        public bool ContentChanged { get; set; }
    }
}

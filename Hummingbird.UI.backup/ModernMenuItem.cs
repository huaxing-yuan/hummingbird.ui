// <copyright file="ModernMenuItem.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-7-9  17:51</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Markup;

namespace Hummingbird.UI
{
    /// <summary>
    /// The MVVM style Menu Item used in ModernWindow.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Control" />
    /// <remarks>
    /// ModernWindow has a special menu system with 3 levels: Level 1 and Level 2 is on the top-left corner, showing horizontally. Level 3 is on the left side.
    /// Each MenuItem has a header which represents its title, then a ContentType object which holds a Type object. The Type object must be a subclass of ModernContent
    /// </remarks>
    /// <see cref="ModernWindow.MainMenu"/>
    [ContentProperty("Items")]
	public class ModernMenuItem : Control
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ModernMenuItem"/> class.
        /// </summary>
        public ModernMenuItem()
		{
			Items = new List<ModernMenuItem>();
		}

        /// <summary>
        /// The items property
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(List<ModernMenuItem>), typeof(ModernMenuItem) );
		
        /// <summary>
        /// Sub menu items of the current MenuItem.
        /// </summary>
        /// <remarks>
        /// Hummingbird UI supports 3 level menu. All sub-items of the 3rd level menu item will be ignored and will not be shown in the User interface
        /// </remarks>
		public List<ModernMenuItem> Items{
			get{ return (List<ModernMenuItem>)GetValue(ItemsProperty);}
			set{ SetValue(ItemsProperty, value);}
		}

        /// <summary>
        /// The header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(ModernMenuItem));

        /// <summary>
        /// Header of the UI, the text to be shown.
        /// </summary>
        /// <remarks>
        /// Design recommendation: For first level menu item, use full lower case characters. For 2nd and 3rd level menu item, use full UPPER case characters.
        /// </remarks>
        public string Header{
			get{ return (string)GetValue(HeaderProperty);}
			set{SetValue(HeaderProperty, value);}
		}

        /// <summary>
        /// The is selected property
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(ModernMenuItem));

        /// <summary>
        /// If the current menu item is selected.
        /// </summary>
        public bool IsSelected{
			get{return (bool)GetValue(IsSelectedProperty);}
			internal set{SetValue(IsSelectedProperty, value);}
		}
		
		internal static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(TimeSpan), typeof(ModernMenuItem));
        
        /// <summary>
        /// Delay indicator when showing animation. Internal use only
        /// </summary>
        internal TimeSpan Delay{
			get{return (TimeSpan)GetValue(DelayProperty);}
			set{SetValue(DelayProperty, value);}
		}
		
        /// <summary>
        /// Parent Menu Item, for internal use only
        /// </summary>
		internal new ModernMenuItem Parent{get;set;}

        /// <summary>
        /// Index of the menu item in the same level, for internal use only
        /// </summary>
        internal int Index{get;set;}

        /// <summary>
        /// Identifier of the MenuItem. The Key is used especially to navigate to 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// A subtype of ModernContent, represents the content of this menu item. Hummingbird UI uses MVVM techniques to manage Menu and the instantiation of the content. To increases the performance, once the view is instantiated, it will be cached.
        /// </summary>
        public Type ContentType{get;set;}
	}
}

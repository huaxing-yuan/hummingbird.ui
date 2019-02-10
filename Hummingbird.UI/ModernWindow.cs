// <copyright file="ModernWindow.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2018-4-19  23:39</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hummingbird.UI
{
    /// <summary>
    /// ModernWindow, represents Modern UI style Window object, which holds its contents in MVVM techniques.
    /// </summary>
    public class ModernWindow : BasicWindow
    {
        readonly System.Collections.Stack recentActions = new System.Collections.Stack();
        static Dictionary<ModernMenuItem, ModernContent> Cache = new Dictionary<ModernMenuItem, ModernContent>();
        static Dictionary<DetachedWindowHost, ModernContent> DetachedContent = new Dictionary<DetachedWindowHost, ModernContent>();
        readonly private ContentDetached contentDetached = new ContentDetached();


        /// <summary>
        /// Release the memory for cached objects, and start immediate a Garbage collection.
        /// </summary>
        /// <remarks>
        /// Hummingbird UI holds the loaded contents in the cache to increase the performance, but it uses memory.
        /// </remarks>
        public void ClearCache()
        {
            ModernContent currentContent = this.Content as ModernContent;
            foreach (var v in Cache.Values)
            {
                if (v != currentContent) v.Dispose();
            }
            Cache.Clear();
        }

        /// <summary>
        /// Invokes the AvatarClicked event .
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        internal void InvokeAvatarClicked(object sender, MouseEventArgs e)
        {
            AvatarClicked?.Invoke(sender, e);
        }

        /// <summary>
        /// Shows the backstage control. Backstage is intended to hold application setting, like what Office 2016 did.
        /// </summary>
        public void ShowBackstage()
        {
            var template = this.Template;
            Border container = template.FindName("BackstageContainer", this) as Border;
            if (container.Visibility == Visibility.Collapsed) container.Visibility = Visibility.Visible;
        }

        private readonly Storyboard HideStatusbarStoryboard, ShowStatusbarStoryboard;
        private readonly Storyboard HideContentLeftStoryBoard, HideContentRightStoryBoard, FadeOutStoryBoard;
        private StatusBar nextStatusBar;

        internal ModernMenuItem loadedMenu = null;
        private bool suppressCanBeClosed;





        /// <summary>
        /// Gets or sets the text shown at the left side of the avatar
        /// </summary>
        /// <value>
        /// The avatar text.
        /// </value>
        public string AvatarText
        {
            get { return (string)GetValue(AvatarTextProperty); }
            set { SetValue(AvatarTextProperty, value); }
        }

        /// <summary>
        ///  Using a DependencyProperty as the backing store for AvatarText.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty AvatarTextProperty =
            DependencyProperty.Register("AvatarText", typeof(string), typeof(ModernWindow), new PropertyMetadata(string.Empty));




        /// <summary>
        /// Sub function of content loading
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="AnimationShown">if set to <c>true</c> </param>
        /// <param name="story">The StoryBoard</param>
        /// <returns></returns>
        private bool LoadContentSub(ModernContent content, out bool AnimationShown, Storyboard story)
        {
            AnimationShown = false;
            if (content.IsDetachable)
            {
                this.IsContentDetachable = true;
                if (DetachedContent.ContainsValue(content))
                {
                    this.IsContentDetached = true;
                    this.Content = contentDetached;
                    return true;
                }
                else
                {
                    this.IsContentDetached = false;
                }
            }
            else
            {
                this.IsContentDetachable = false;
                this.IsContentDetached = false;
            }
            this.Content = content;
            this.CommandArea = content.CommandArea;

            if (content.StatusBar != null)
            {
                nextStatusBar = content.StatusBar;
            }
            else
            {
                nextStatusBar = this.DefaultStatusBar;
            }
            if (this.StatusBar != nextStatusBar)
            {
                if (this.StatusBar == null)
                {
                    this.StatusBar = nextStatusBar;
                    if (this.StatusBar != null)
                    {
                        this.StatusBar.BeginStoryboard(ShowStatusbarStoryboard);
                        System.Diagnostics.Debug.WriteLine("BeginStoryboard Show line 79");
                    }
                }
                else
                {
                    this.StatusBar.BeginStoryboard(HideStatusbarStoryboard);
                    System.Diagnostics.Debug.WriteLine("BeginStoryboard Hide line 85");
                }
            }

            content.BeginStoryboard(story);
            AnimationShown = true;
            System.Diagnostics.Debug.WriteLine("BeginStoryboard Show content");
            return false;
        }

        internal bool LoadContent(ModernMenuItem item, out bool isLeft, out bool AnimationShown)
        {
            int previousIndex, currentIndex;
            currentIndex = item.Index;
            previousIndex = 0;
            if (recentActions.Count > 0)
            {
                ModernMenuItem previousItem = recentActions.Peek() as ModernMenuItem;
                previousIndex = previousItem.Index;
            }
            isLeft = currentIndex <= previousIndex;
            System.Diagnostics.Debug.WriteLine("IsLeft=" + isLeft);
            AnimationShown = false;
            if (item.Items != null && item.Items.Count > 0) return true;


            if (this.Content is ModernContent currentContent && !suppressCanBeClosed && !currentContent.CanBeClosed())
            {
                return false;
            }




            recentActions.Push(item);
            if (item.ContentType != null && item.ContentType.IsSubclassOf(typeof(ModernContent)))
            {
                Storyboard story = isLeft ? ((Storyboard)FindResource("ContentLeftInStoryboard")).Clone() : ((Storyboard)FindResource("ContentRightInStoryboard")).Clone();
                if (Cache.ContainsKey(item))
                {
                    //item has already in the Cache
                    ModernContent content = Cache[item];
                    loadedMenu = item;
                    if (LoadContentSub(content, out AnimationShown, story)) return true;
                }
                else
                {
                    ModernContent content = (ModernContent)Activator.CreateInstance(item.ContentType);
                    Cache.Add(item, content);
                    loadedMenu = item;
                    if (LoadContentSub(content, out AnimationShown, story)) return true;
                }

            }
            else
            {
                this.Content = null;
                this.CommandArea = null;
                this.IsContentDetachable = false;
                this.IsContentDetached = false;
                loadedMenu = item;
                nextStatusBar = DefaultStatusBar;
                if (this.StatusBar != nextStatusBar)
                {
                    if (this.StatusBar != null)
                    {
                        this.StatusBar.BeginStoryboard(HideStatusbarStoryboard);
                        System.Diagnostics.Debug.WriteLine("Hide StatueBar line 190");
                    }
                    else
                    {
                        this.StatusBar = nextStatusBar;
                        if (this.StatusBar != null)
                        {
                            this.StatusBar.BeginStoryboard(ShowStatusbarStoryboard);
                            System.Diagnostics.Debug.WriteLine("Show StatusBar line 198");
                        }
                    }
                }
            }
            return true;
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="ModernWindow"/> class.
        /// </summary>
        public ModernWindow()
        {
            HasControlBox = true;
            MainMenu = new ObservableCollection<ModernMenuItem>();
            MainMenuLevel2 = new ObservableCollection<ModernMenuItem>();
            MainMenuLevel3 = new ObservableCollection<ModernMenuItem>();
            StatusBar = new StatusBar();
            CommandArea = new StackPanel() { Orientation = Orientation.Horizontal };
            DefaultStyle = "DefaultModernWindow";
            this.Initialized += WindowInitialized;
            this.Loaded += ModernWindow_Loaded;
            HideStatusbarStoryboard = ((Storyboard)FindResource("HideStatusbarStoryboard")).Clone();
            ShowStatusbarStoryboard = ((Storyboard)FindResource("ShowStatusbarStoryboard")).Clone();
            ShowStatusbarStoryboard.BeginTime = new TimeSpan(0, 0, 0, 0, 0);
            ShowStatusbarStoryboard.Completed += ShowStatusbarStoryboard_Completed;
            HideStatusbarStoryboard.BeginTime = new TimeSpan(0, 0, 0, 0, 0);
            HideStatusbarStoryboard.Completed += HideStatusbarStoryboard_Completed;
            HideContentLeftStoryBoard = ((Storyboard)FindResource("HideContentLeftStoryBoard")).Clone();
            HideContentRightStoryBoard = ((Storyboard)FindResource("HideContentRightStoryBoard")).Clone();
            FadeOutStoryBoard = ((Storyboard)FindResource("FadeOutStoryBoard")).Clone();
            this.Closing += ModernWindow_Closing;
        }

        private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.Content is ModernContent)
            {
                ModernContent currentContent = this.Content as ModernContent;
                if (currentContent.CanBeClosed())
                {
                    e.Cancel = false;
                }
                else
                {
                    //The current content prevents close.
                    e.Cancel = true;
                }

            }
        }

        private async void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //Level 1
            int index = 0;
            if (this.MainMenu != null)
            {
                foreach (ModernMenuItem item in this.MainMenu)
                {
                    item.Index = index++;
                    item.MouseDown += MainMenuItem_MouseDown;
                    if (item.Items != null)
                    {
                        //Level 2
                        foreach (ModernMenuItem item2 in item.Items)
                        {
                            item2.MouseDown += MainMenuItemLevel2_MouseDown;
                            item2.Parent = item;
                            item2.Index = index++;
                            if (item2.Items != null)
                            {
                                foreach (ModernMenuItem item3 in item2.Items)
                                {
                                    item3.MouseDown += MainMenuItemLevel3_MouseDown;
                                    item3.Parent = item2;
                                    item3.LayoutTransform = new TranslateTransform();
                                    item3.Index = index++;
                                }
                            }
                        }
                    }
                }
            }

            if (this.MainMenu != null && MainMenu.Count > 0)
            {
                await SelectMenuItem_Level1(MainMenu[0]);
            }
        }

        private void ShowStatusbarStoryboard_Completed(object sender, EventArgs e)
        {
            this.StatusBar.Opacity = 1;
        }




        /// <summary>Gets or sets a value indicating whether the avatar is visible.</summary>
        /// <value>
        ///   <c>true</c> if the avatar is visible; otherwise, <c>false</c>.</value>
        public bool IsAvatarVisible
        {
            get { return (bool)GetValue(IsAvatarVisibleProperty); }
            set { SetValue(IsAvatarVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAvatarVisible.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="IsAvatarVisible"/>
        /// </summary>
        public static readonly DependencyProperty IsAvatarVisibleProperty =
            DependencyProperty.Register("IsAvatarVisible", typeof(bool), typeof(ModernWindow), new PropertyMetadata(false));




        /// <summary>
        /// Gets or sets the size of the avatar icon.
        /// </summary>
        /// <value>
        /// The size of the avatar.
        /// </value>
        public double AvatarSize
        {
            get { return (double)GetValue(AvatarSizeProperty); }
            set { SetValue(AvatarSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvatarSize.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="AvatarSize"/>
        /// </summary>
        public static readonly DependencyProperty AvatarSizeProperty =
            DependencyProperty.Register("AvatarSize", typeof(double), typeof(ModernWindow), new PropertyMetadata(48d));

        /// <summary>
        /// Occurs when Avatar icon is clicked.
        /// </summary>
        public event MouseEventHandler AvatarClicked;

        /// <summary>
        /// Gets or sets the avatar brush. it can be any type of <see cref="Brush"/> that will be shown as Avatar if <see cref="IsAvatarVisible"/> is set to <see langword="true"/>
        /// </summary>
        /// <value>
        /// The avatar brush.
        /// </value>
        public Brush AvatarBrush
        {
            get { return (Brush)GetValue(AvatarBrushProperty); }
            set { SetValue(AvatarBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvatarBrush.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="AvatarBrush"/>
        /// </summary>
        public static readonly DependencyProperty AvatarBrushProperty =
            DependencyProperty.Register("AvatarBrush", typeof(Brush), typeof(ModernWindow), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));


        private void HideStatusbarStoryboard_Completed(object sender, EventArgs e)
        {
            this.StatusBar = nextStatusBar;
            if (nextStatusBar != null)
            {
                this.StatusBar.Opacity = 0;
                double height = this.ActualHeight;
                this.StatusBar.LayoutTransform = new ScaleTransform(1, 1, 0, height);
                this.StatusBar.RenderTransform = new ScaleTransform(1, 1, 0, height);
                this.StatusBar.BeginStoryboard(ShowStatusbarStoryboard);
                System.Diagnostics.Debug.WriteLine("Show Status bar Storyboard");
            }
        }

        /// <summary>
        /// The main menu property
        /// </summary>
        public static readonly DependencyProperty MainMenuProperty = DependencyProperty.Register("MainMenu", typeof(ObservableCollection<ModernMenuItem>), typeof(ModernWindow));

        /// <summary>
        /// Main menu of the ModernWindow. Hummingbird UI holds a 3-level-menu system. 
        /// </summary>
        public ObservableCollection<ModernMenuItem> MainMenu
        {
            get { return (ObservableCollection<ModernMenuItem>)GetValue(MainMenuProperty); }
            set { SetValue(MainMenuProperty, value); }
        }

        internal static readonly DependencyProperty MainMenuLevel2Property = DependencyProperty.Register("MainMenuLevel2", typeof(ObservableCollection<ModernMenuItem>), typeof(ModernWindow));
        internal ObservableCollection<ModernMenuItem> MainMenuLevel2
        {
            get { return (ObservableCollection<ModernMenuItem>)GetValue(MainMenuLevel2Property); }
            set
            {
                SetValue(MainMenuLevel2Property, value);
            }
        }

        internal static readonly DependencyProperty MainMenuLevel3Property = DependencyProperty.Register("MainMenuLevel3", typeof(ObservableCollection<ModernMenuItem>), typeof(ModernWindow));
        internal ObservableCollection<ModernMenuItem> MainMenuLevel3
        {
            get { return (ObservableCollection<ModernMenuItem>)GetValue(MainMenuLevel3Property); }
            set { SetValue(MainMenuLevel3Property, value); }
        }



        /// <summary>
        /// The backstage panel property
        /// </summary>
        public static readonly DependencyProperty BackstagePanelProperty = DependencyProperty.Register("BackstagePanel", typeof(TabControl), typeof(ModernWindow));

        /// <summary>
        /// A TabControl object shown as backstage panel. usually this panel can be used to adjust application settings.
        /// </summary>
        public TabControl BackstagePanel
        {
            get
            {
                return (TabControl)GetValue(BackstagePanelProperty);
            }
            set
            {
                SetValue(BackstagePanelProperty, value);
            }
        }


        /// <summary>
        /// The status bar property
        /// </summary>
        protected static readonly DependencyProperty StatusBarProperty = DependencyProperty.Register("StatusBar", typeof(StatusBar), typeof(ModernWindow));

        /// <summary>
        /// StatusBar of the ModernWindow. Used like the CommandBar, intended to hold several <see cref="AppBarButton"/> objects.
        /// </summary>
        protected StatusBar StatusBar
        {
            get
            {
                return (StatusBar)GetValue(StatusBarProperty);
            }
            set
            {
                SetValue(StatusBarProperty, value);
            }
        }

        /// <summary>
        /// Default status bar of ModernWindow, when an ModernContent has not defined status bar, this status bar will be shown.
        /// </summary>
        public StatusBar DefaultStatusBar { get; set; }
        private void WindowInitialized(object sender, EventArgs e)
        {



            //Backstage Style
            if (BackstagePanel != null)
            {
                BackstagePanel.Style = (Style)FindResource("BackstageTabControlStyle");
            }

        }

        async void MainMenuItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ModernMenuItem item = sender as ModernMenuItem;
            if (this.Content != null && item.ContentType == this.Content?.GetType()) return;
            if (!(this.Content is ModernContent currentContent) || currentContent.CanBeClosed())
            {
                suppressCanBeClosed = true;
                await SelectMenuItem_Level1(item);
                suppressCanBeClosed = false;
            }
        }

        void MainMenuItemLevel2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ModernMenuItem item = sender as ModernMenuItem;
            if (this.Content != null && item.ContentType == this.Content?.GetType()) return;
            SelectMenuItem_Level2(item);
        }

        void MainMenuItemLevel3_MouseDown(object sender, MouseEventArgs e)
        {
            ModernMenuItem item = sender as ModernMenuItem;
            if (this.Content != null && item.ContentType == this.Content?.GetType()) return;
            SelectMenuItem_Level3(item);
        }

        async Task SelectMenuItem_Level1(ModernMenuItem selectedItem, string level2ToSelect = null, string level3ToSelect = null)
        {
            ModernMenuItem previousItem = null;
            foreach (ModernMenuItem item in this.MainMenu)
            {
                if (item.IsSelected)
                {
                    previousItem = item;
                    break;
                }
            }

            if (LoadContent(selectedItem, out bool isLeft, out bool AnimationShown))
            {
                if (previousItem != selectedItem || level2ToSelect != null)
                {
                    if (previousItem != null)
                    {
                        previousItem.IsSelected = false;
                    }

                    selectedItem.IsSelected = true;
                    this.MainMenuLevel2.Clear();
                    this.MainMenuLevel3.Clear();
                    if (selectedItem.Items != null && selectedItem.Items.Count > 0)
                    {
                        int count = selectedItem.Items.Count;
                        ModernMenuItem selectedLevel2Item = null;
                        if (!AnimationShown) HideContent(isLeft);
                        await Task.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {

                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    ModernMenuItem current = selectedItem.Items[i];
                                    this.MainMenuLevel2.Add(current);
                                    if (current.IsSelected && level2ToSelect == null)
                                    {
                                        selectedLevel2Item = current;
                                    }
                                    else if (level2ToSelect == current.Key)
                                    {
                                        selectedLevel2Item = current;
                                        selectedLevel2Item.IsSelected = true;
                                    }
                                    else
                                    {
                                        current.IsSelected = false;
                                    }
                                }

                                ));
                                Thread.Sleep(50);
                            }
                        }
                        );
                        if (selectedLevel2Item != null)
                        {
                            SelectMenuItem_Level2(selectedLevel2Item, level3ToSelect);
                        }
                        else
                        {
                            SelectMenuItem_Level2(selectedItem.Items[0]);
                        }
                    }
                }
                else if (level2ToSelect != null && selectedItem.Items != null && selectedItem.Items.Count > 0)
                {
                    //The Level1 of the KeyPath is the same, so we need to select the second level item
                    int count = selectedItem.Items.Count;
                    ModernMenuItem selectedLevel2Item = null;
                    await Task.Run(() =>
                    {
                        this.Content = null;
                        for (int i = 0; i < count; i++)
                        {

                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                ModernMenuItem current = selectedItem.Items[i];
                                if (current.IsSelected && level2ToSelect == null)
                                {
                                    selectedLevel2Item = current;
                                }
                                else if (level2ToSelect == current.Key)
                                {
                                    selectedLevel2Item = current;
                                    selectedLevel2Item.IsSelected = true;
                                }
                                else
                                {
                                    current.IsSelected = false;
                                }
                            }

                            ));
                            Thread.Sleep(50);
                        }
                    }
                    );
                    if (selectedLevel2Item != null)
                    {
                        SelectMenuItem_Level2(selectedLevel2Item, level3ToSelect);
                    }
                    else
                    {
                        SelectMenuItem_Level2(selectedItem.Items[0]);
                    }

                }

            }
        }

        private void HideContent(bool? isLeft)
        {
            if (this.Content is ModernContent mc)
            {
                mc.RenderTransform = new TranslateTransform();
            }
            if (isLeft.HasValue)
            {
                if (isLeft.Value)
                {
                    (this.Content as ModernContent)?.BeginStoryboard(HideContentRightStoryBoard);
                    System.Diagnostics.Debug.WriteLine("Hide Content with Animation Left");
                }
                else
                {
                    (this.Content as ModernContent)?.BeginStoryboard(HideContentLeftStoryBoard);
                    System.Diagnostics.Debug.WriteLine("Hide Content with Animation Right");
                }
            }
            else
            {
                (this.Content as ModernContent)?.BeginStoryboard(FadeOutStoryBoard);
                System.Diagnostics.Debug.WriteLine("Hide Content with Animation Fadeout");
            }
            Thread.Sleep(200);
        }

        void SelectMenuItem_Level2(ModernMenuItem selectedItem, string level3ToSelect = null)
        {
            ModernMenuItem previousItem = null;
            foreach (ModernMenuItem item in selectedItem.Parent.Items)
            {
                if (item.IsSelected)
                {
                    previousItem = item;
                    break;
                }
            }
            if (LoadContent(selectedItem, out bool isLeft, out bool AnimationShown))
            {
                if (previousItem != selectedItem || (this.MainMenuLevel3.Count == 0 && selectedItem.Items != null))
                {
                    if (previousItem != null)
                    {
                        previousItem.IsSelected = false;
                    }

                    selectedItem.IsSelected = true;
                    this.MainMenuLevel3.Clear();
                    if (selectedItem.Items != null && selectedItem.Items.Count > 0)
                    {
                        SelectMenuLevel3FromLevel2(selectedItem, level3ToSelect);
                    }
                }
                else if (level3ToSelect != null && selectedItem.Items != null && selectedItem.Items.Count > 0)  //The Level2 of the KeyPath is the same, but we still need to select the 3rd level item
                {
                    SelectMenuLevel3FromLevel2(selectedItem, level3ToSelect);
                }
            }
        }

        private void SelectMenuLevel3FromLevel2(ModernMenuItem selectedItem, string level3ToSelect)
        {
            ModernMenuItem selectedLevel3Item = null;
            foreach (var item3 in selectedItem.Items)
            {
                this.MainMenuLevel3.Add(item3);
                if (item3.IsSelected && level3ToSelect == null)
                {
                    selectedLevel3Item = item3;
                }
                else if (level3ToSelect == item3.Key)
                {
                    selectedLevel3Item = item3;
                    selectedLevel3Item.IsSelected = true;
                }
                else
                {
                    item3.IsSelected = false;
                }
            }

            ItemsControl ic = this.Template.FindName("ItemsControl", this) as ItemsControl;
            ic.BeginStoryboard((Storyboard)FindResource("ContentLeftInStoryboard"));

            if (selectedLevel3Item != null)
            {
                SelectMenuItem_Level3(selectedLevel3Item);
            }
            else
            {
                SelectMenuItem_Level3(selectedItem.Items[0]);
            }

        }

        void SelectMenuItem_Level3(ModernMenuItem selectedItem)
        {
            ModernMenuItem previousItem = null;
            foreach (ModernMenuItem item in selectedItem.Parent.Items)
            {
                if (item.IsSelected)
                {
                    previousItem = item;
                    break;
                }
            }
            if (LoadContent(selectedItem, out bool isLeft, out bool AnimationShown) && previousItem != selectedItem)
            {
                if (previousItem != null)
                {
                    previousItem.IsSelected = false;
                }
                selectedItem.IsSelected = true;
            }

        }


        /// <summary>
        /// A little area which present on the upper-right corner, on the same level as MainMenu.
        /// </summary>
        public StackPanel CommandArea
        {
            get { return (StackPanel)GetValue(CommandAreaProperty); }
            set { SetValue(CommandAreaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandArea.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The command area property
        /// </summary>
        public static readonly DependencyProperty CommandAreaProperty =
            DependencyProperty.Register("CommandArea", typeof(StackPanel), typeof(ModernWindow));


        private string CurrentPath()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(loadedMenu.Key);
            var current = loadedMenu;

            while (current.Parent != null)
            {
                current = current.Parent;
                sb.Insert(0, current.Key + "|");
            }
            return sb.ToString();
        }


        /// <summary>
        /// Navigate to a select menu item by the key path.
        /// KeyPath is the path composite with the Key of ModernItem separated with |
        /// </summary>
        /// <param name="KeyPath">The KeyPath of menu Items</param>
        /// <returns>the Task object</returns>
        /// <example>
        /// Giving Menu Items MENU1 which has sub items as MENU1_1 and MENU1_2. if you want to navigate directly to MENU1_2, you can call:
        /// NavigateTo("MENU1|MENU1_2")
        /// </example>
        public async Task NavigateTo(string KeyPath)
        {
            string[] levels = KeyPath.Split('|');
            string[] currentPath = CurrentPath().Split('|');
            var item = FindMenuItemByPath(levels, this.MainMenu.ToArray(), 0, out int level);
            if (level == 0)
            {
                MainMenuItem_MouseDown(item, null);
            }
            else if (level == 1)
            {
                if (currentPath.Length > 1 && currentPath[0] == levels[0])
                {
                    MainMenuItemLevel2_MouseDown(item, null);
                }
                else
                {
                    await SelectMenuItem_Level1(item.Parent, item.Key);
                }
            }
            else if (level == 2)
            {
                if (currentPath.Length > 2 && currentPath[0] == levels[0] && currentPath[1] == levels[1])
                {
                    MainMenuItemLevel3_MouseDown(item, null);
                }
                else if (currentPath.Length > 2 && currentPath[0] == levels[0])
                {
                    SelectMenuItem_Level2(item.Parent, levels[2]);
                }
                else
                {
                    await SelectMenuItem_Level1(item.Parent.Parent, levels[1], levels[2]);
                }
            }

        }

        private ModernMenuItem FindMenuItemByPath(IEnumerable<string> Path, ModernMenuItem[] item, int inlevel, out int level)
        {
            level = inlevel;
            if (Path.Any())
            {
                var selected = item.FirstOrDefault(x => x.Key == Path.First());
                if (selected != null && Path.Count() > 1)
                {
                    return FindMenuItemByPath(Path.Skip(1), selected.Items.ToArray(), level + 1, out level);
                }
                else
                {
                    return selected;
                }
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Gets the value if the current active content is detachable
        /// </summary>
        public bool IsContentDetachable
        {
            get { return (bool)GetValue(IsContentDetachableProperty); }
            private set { SetValue(IsContentDetachableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsContentDetachable.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The is content detachable property
        /// </summary>
        public static readonly DependencyProperty IsContentDetachableProperty =
            DependencyProperty.Register("IsContentDetachable", typeof(bool), typeof(ModernWindow), new PropertyMetadata(false));



        /// <summary>
        /// Gets the value if the current active content has already detached.
        /// </summary>
        public bool IsContentDetached
        {
            get { return (bool)GetValue(IsContentDetachedProperty); }
            private set { SetValue(IsContentDetachedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsContentDetached.  This enables animation, styling, binding, etc...
        /// <summary>
        /// The is content detached property
        /// </summary>
        public static readonly DependencyProperty IsContentDetachedProperty =
            DependencyProperty.Register("IsContentDetached", typeof(bool), typeof(ModernWindow), new PropertyMetadata(false));

        internal void DetachContent()
        {
            ModernContent content = this.Content as ModernContent;
            this.Content = null;
            this.CommandArea = null;
            if (content.StatusBar != null) this.StatusBar = null;
            DetachedWindowHost host = new DetachedWindowHost(content.DetachedName, content, detachedWindowClosed);
            DetachedContent.Add(host, content);
            host.Owner = this;
            host.Show();
            if (host.Width < this.Width && host.Width < 1152) host.Width = 1000;
            if (host.Height < this.Height && host.Height < 800) host.Height = 800;
            this.IsContentDetached = true;
            this.Content = contentDetached;
        }

        private void detachedWindowClosed(object sender, EventArgs e)
        {
            DetachedWindowHost host = sender as DetachedWindowHost;
            ModernContent mc = DetachedContent[host];
            DetachedContent.Remove(host);

            if (loadedMenu.ContentType == mc.GetType() && this.Content == contentDetached)
            {
                LoadContent(loadedMenu, out bool isLeft, out bool AnimationShown);
            }
        }

        internal void AttachContent()
        {
            this.IsContentDetached = false;
        }


    }
}

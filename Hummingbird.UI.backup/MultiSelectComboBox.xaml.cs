// <copyright file="MultiSelectComboBox.xaml.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2017-6-8  22:58</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Hummingbird.UI
{
    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox : ComboBox
    {
        private ObservableCollection<Node> _nodeList;
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectComboBox"/> class.
        /// </summary>
        public MultiSelectComboBox()
        {
            InitializeComponent();
            _nodeList = new ObservableCollection<Node>();
        }

        #region Dependency Properties

        /// <summary>
        /// The items source property
        /// </summary>
        public static readonly DependencyProperty ItemsSource2Property =
             DependencyProperty.Register("ItemsSource", typeof(Dictionary<string, object>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(MultiSelectComboBox.OnItemsSourceChanged)));

        /// <summary>
        /// The selected items property
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
         DependencyProperty.Register("SelectedItems", typeof(Dictionary<string, object>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
     new PropertyChangedCallback(MultiSelectComboBox.OnSelectedItemsChanged)));

        /// <summary>
        /// The text property
        /// </summary>
        public static new readonly DependencyProperty  TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// The default text property
        /// </summary>
        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));



        /// <summary>
        /// Gets or sets a collection used to generate the content of the <see cref="T:System.Windows.Controls.ItemsControl" />.
        /// </summary>
        public new Dictionary<string, object> ItemsSource
        {
            get { return (Dictionary<string, object>)GetValue(ItemsSource2Property); }
            set
            {
                SetValue(ItemsSource2Property, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        /// <value>
        /// The selected items.
        /// </value>
        public Dictionary<string, object> SelectedItems
        {
            get { return (Dictionary<string, object>)GetValue(SelectedItemsProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text of the currently selected item.
        /// </summary>
        public new string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { 
                SetValue(TextProperty, value);
                SelectedItemsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the default text.
        /// </summary>
        /// <value>
        /// The default text.
        /// </value>
        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }
        #endregion

        #region Events
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.DisplayInControl();
            control.SetText();
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBox control = (MultiSelectComboBox)d;
            control.SelectNodes();
            control.SetText();
        }

        /// <summary>
        /// Occurs when selected items changed.
        /// </summary>
        public event EventHandler SelectedItemsChanged;

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox clickedBox = (CheckBox)sender;

            if ("All" == (string)clickedBox.Content )
            {
                if (clickedBox.IsChecked.Value)
                {
                    foreach (Node node in _nodeList)
                    {
                        node.IsSelected = true;
                    }
                }
                else
                {
                    foreach (Node node in _nodeList)
                    {
                        node.IsSelected = false;
                    }
                }

            }
            else
            {
                int _selectedCount = 0;
                foreach (Node s in _nodeList)
                {
                    if (s.IsSelected && s.Title != "All")
                        _selectedCount++;
                }
                if (_selectedCount == _nodeList.Count - 1)
                    _nodeList.FirstOrDefault(i => i.Title == "All").IsSelected = true;
                else
                    _nodeList.FirstOrDefault(i => i.Title == "All").IsSelected = false;
            }
            SetSelectedItems();
            SetText();

        }
        #endregion

        #region Methods
        private void SelectNodes()
        {
            foreach (KeyValuePair<string, object> keyValue in SelectedItems)
            {
                Node node = _nodeList.FirstOrDefault(i => i.Title == keyValue.Key);
                if (node != null)
                    node.IsSelected = true;
            }
        }

        private void SetSelectedItems()
        {
            if (SelectedItems == null)
                SelectedItems = new Dictionary<string, object>();
            SelectedItems.Clear();
            foreach (Node node in _nodeList)
            {
                if (node.IsSelected && node.Title != "All" && this.ItemsSource.Count > 0)
                {
                    SelectedItems.Add(node.Title, this.ItemsSource[node.Title]);
                }
            }
        }

        private void DisplayInControl()
        {
            _nodeList.Clear();
            if (this.ItemsSource.Count > 0)
                _nodeList.Add(new Node("All"));
            foreach (KeyValuePair<string, object> keyValue in this.ItemsSource)
            {
                Node node = new Node(keyValue.Key);
                _nodeList.Add(node);
            }
            base.ItemsSource = _nodeList;
        }


        private void SetText()
        {
            if (this.SelectedItems != null)
            {
                StringBuilder displayText = new StringBuilder();
                foreach (Node s in _nodeList)
                {
                    if (s.IsSelected && s.Title == "All")
                    {
                        displayText = new StringBuilder();
                        displayText.Append("All");
                        break;
                    }
                    else if (s.IsSelected && s.Title != "All")
                    {
                        displayText.Append(s.Title);
                        displayText.Append(", ");
                    }
                }
                this.Text = displayText.ToString().TrimEnd(new char[] { ',', ' ' }); 
            }           
            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(this.Text))
            {
                this.Text = this.DefaultText;
            }
        }

       
        #endregion
    }

    /// <summary>
    /// Node in a MultiSelectComboBox
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class Node : INotifyPropertyChanged
    {

        private string _title;
        private bool _isSelected;
        #region ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public Node(string title)
        {
            Title = title;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged(nameof(Title));
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged(nameof(IsSelected));
            }
        }
        #endregion

        /// <summary>
        /// Occurs when property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notifies all subscribers of PropertyChanged event listener that the property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

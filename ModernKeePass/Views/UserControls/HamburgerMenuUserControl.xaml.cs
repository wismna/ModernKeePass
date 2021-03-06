﻿using System;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Controls;
using System.Collections.ObjectModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class HamburgerMenuUserControl
    {
        public string HeaderLabel
        {
            get { return (string)GetValue(HeaderLabelProperty); }
            set { SetValue(HeaderLabelProperty, value); }
        }
        public static readonly DependencyProperty HeaderLabelProperty =
            DependencyProperty.Register(
                nameof(HeaderLabel),
                typeof(string),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata("Header", (o, args) => { }));
        
        public string ButtonLabel
        {
            get { return (string)GetValue(ButtonLabelProperty); }
            set { SetValue(ButtonLabelProperty, value); }
        }
        public static readonly DependencyProperty ButtonLabelProperty =
            DependencyProperty.Register(
                nameof(ButtonLabel),
                typeof(string),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata("Button", (o, args) => { }));

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(
                nameof(DisplayMemberPath),
                typeof(string),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata("Title", (o, args) => { }));
        
        public Visibility IsButtonVisible
        {
            get { return (Visibility)GetValue(IsButtonVisibleProperty); }
            set { SetValue(IsButtonVisibleProperty, value); }
        }
        public static readonly DependencyProperty IsButtonVisibleProperty =
            DependencyProperty.Register(
                nameof(IsButtonVisible),
                typeof(Visibility),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(Visibility.Collapsed, (o, args) => { }));

        public ObservableCollection<IEntityVm> ItemsSource
        {
            get { return (ObservableCollection<IEntityVm>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(ObservableCollection<IEntityVm>),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(new ObservableCollection<IEntityVm>(), (o, args) => { }));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(
                nameof(SelectedIndex),
                typeof(int),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(-1, (o, args) => { }));
        
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(
                nameof(IsOpen),
                typeof(bool),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(false, (o, args) => { }));
  
        public bool CanDragItems
        {
            get { return (bool)GetValue(CanDragItemsProperty); }
            set { SetValue(CanDragItemsProperty, value); }
        }
        public static readonly DependencyProperty CanDragItemsProperty =
            DependencyProperty.Register(
                nameof(CanDragItems),
                typeof(bool),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(false, (o, args) => { }));

        public ICommand ActionButtonCommand
        {
            get { return (ICommand)GetValue(ActionButtonCommandProperty); }
            set { SetValue(ActionButtonCommandProperty, value); }
        }
        public static readonly DependencyProperty ActionButtonCommandProperty =
            DependencyProperty.Register(
                nameof(ActionButtonCommand),
                typeof(ICommand),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));
        
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
        
        public HamburgerMenuUserControl()
        {
            InitializeComponent();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            var parent = Parent as FrameworkElement;
            if (parent == null) return;
            VisualStateManager.GoToState(this, parent.ActualWidth <= 640 ? "Hidden" : "Collapsed", true);
        }

        private void NewGroupTextBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter) return;
            var textBox = sender as TextBoxWithButton;
            ActionButtonCommand.Execute(textBox?.Text);
            // Stop the event from triggering twice
            e.Handled = true;
        }
    }
}

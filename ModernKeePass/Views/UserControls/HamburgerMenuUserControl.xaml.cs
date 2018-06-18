﻿using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Interfaces;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class HamburgerMenuUserControl
    {
        public HamburgerMenuUserControl()
        {
            InitializeComponent();
        }

        public string HeaderLabel
        {
            get { return (string)GetValue(HeaderLabelProperty); }
            set { SetValue(HeaderLabelProperty, value); }
        }
        public static readonly DependencyProperty HeaderLabelProperty =
            DependencyProperty.Register(
                "HeaderLabel",
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
                "ButtonLabel",
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
                "DisplayMemberPath",
                typeof(string),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata("Title", (o, args) => { }));

        public object ResizeTarget
        {
            get { return GetValue(ResizeTargetProperty); }
            set { SetValue(ResizeTargetProperty, value); }
        }
        public static readonly DependencyProperty ResizeTargetProperty =
            DependencyProperty.Register(
                "ResizeTarget",
                typeof(object),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public Visibility IsButtonVisible
        {
            get { return (Visibility)GetValue(IsButtonVisibleProperty); }
            set { SetValue(IsButtonVisibleProperty, value); }
        }
        public static readonly DependencyProperty IsButtonVisibleProperty =
            DependencyProperty.Register(
                "IsButtonVisible",
                typeof(Visibility),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(Visibility.Collapsed, (o, args) => { }));

        public IEnumerable<IPwEntity> ItemsSource
        {
            get { return (IEnumerable<IPwEntity>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable<IPwEntity>),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(new List<IPwEntity>(), (o, args) => { }));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof(object),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public event SelectionChangedEventHandler SelectionChanged;
        public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }

        public event ButtonClickedEventHandler ButtonClicked;
        public delegate void ButtonClickedEventHandler(object sender, RoutedEventArgs e);
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonClicked?.Invoke(sender, e);
        }
    }
}
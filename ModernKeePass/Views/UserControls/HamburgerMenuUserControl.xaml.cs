using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Application.Common.Interfaces;

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

        public object ResizeTarget
        {
            get { return GetValue(ResizeTargetProperty); }
            set { SetValue(ResizeTargetProperty, value); }
        }
        public static readonly DependencyProperty ResizeTargetProperty =
            DependencyProperty.Register(
                nameof(ResizeTarget),
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
                nameof(IsButtonVisible),
                typeof(Visibility),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(Visibility.Collapsed, (o, args) => { }));

        public IEnumerable<IEntityVm> ItemsSource
        {
            get { return (IEnumerable<IEntityVm>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable<IEntityVm>),
                typeof(HamburgerMenuUserControl),
                new PropertyMetadata(new List<IEntityVm>(), (o, args) => { }));

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
                new PropertyMetadata(true, (o, args) => { }));

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }

        public event EventHandler<RoutedEventArgs> ButtonClicked;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonClicked?.Invoke(sender, e);
        }
    }
}

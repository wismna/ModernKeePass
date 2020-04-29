using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModernKeePass.Controls
{
    public class PasswordBoxWithButton: SearchBox
    {
        public event EventHandler<RoutedEventArgs> ButtonClick;

        public string ButtonSymbol
        {
            get { return (string)GetValue(ButtonSymbolProperty); }
            set { SetValue(ButtonSymbolProperty, value); }
        }
        public static readonly DependencyProperty ButtonSymbolProperty =
            DependencyProperty.Register(
                nameof(ButtonSymbol),
                typeof(string),
                typeof(PasswordBoxWithButton),
                new PropertyMetadata("&#xE107;", (o, args) => { }));

        public string ButtonTooltip
        {
            get { return (string)GetValue(ButtonTooltipProperty); }
            set { SetValue(ButtonTooltipProperty, value); }
        }
        public static readonly DependencyProperty ButtonTooltipProperty =
            DependencyProperty.Register(
                nameof(ButtonTooltip),
                typeof(string),
                typeof(PasswordBoxWithButton),
                new PropertyMetadata(string.Empty, (o, args) => { }));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(PasswordBoxWithButton),
                new PropertyMetadata(string.Empty, (o, args) => { }));
        
        public bool IsButtonEnabled
        {
            get { return (bool)GetValue(IsButtonEnabledProperty); }
            set { SetValue(IsButtonEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsButtonEnabledProperty =
            DependencyProperty.Register(
                nameof(IsButtonEnabled),
                typeof(bool),
                typeof(PasswordBoxWithButton),
                new PropertyMetadata(true, (o, args) => { }));

        public bool IsPasswordRevealEnabled
        {
            get { return (bool)GetValue(IsPasswordRevealEnabledProperty); }
            set { SetValue(IsPasswordRevealEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsPasswordRevealEnabledProperty =
            DependencyProperty.Register(
                nameof(IsPasswordRevealEnabled),
                typeof(bool),
                typeof(PasswordBoxWithButton),
                new PropertyMetadata(true, (o, args) => { }));


        public PasswordBoxWithButton()
        {
            DefaultStyleKey = typeof(PasswordBoxWithButton);
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var actionButton = GetTemplateChild("ActionButton") as Button;
            if (actionButton != null)
            {
                actionButton.Click += (sender, e) => ButtonClick?.Invoke(sender, e);
            }
        }
    }
}
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModernKeePass.Controls
{
    public class TextBoxWithButton : TextBox
    {
        public event EventHandler<RoutedEventArgs> ButtonClick;

        public string ButtonSymbol
        {
            get => (string)GetValue(ButtonSymbolProperty);
            set => SetValue(ButtonSymbolProperty, value);
        }
        public static readonly DependencyProperty ButtonSymbolProperty =
            DependencyProperty.Register(
                "ButtonSymbol",
                typeof(string),
                typeof(TextBoxWithButton),
                new PropertyMetadata("&#xE107;", (o, args) => { }));

        public string ButtonTooltip
        {
            get => (string)GetValue(ButtonTooltipProperty);
            set => SetValue(ButtonTooltipProperty, value);
        }
        public static readonly DependencyProperty ButtonTooltipProperty =
            DependencyProperty.Register(
                "ButtonTooltip",
                typeof(string),
                typeof(TextBoxWithButton),
                new PropertyMetadata(string.Empty, (o, args) => { }));

        public bool IsButtonEnabled
        {
            get => (bool)GetValue(IsButtonEnabledProperty);
            set => SetValue(IsButtonEnabledProperty, value);
        }
        public static readonly DependencyProperty IsButtonEnabledProperty =
            DependencyProperty.Register(
                "IsButtonEnabled",
                typeof(bool),
                typeof(TextBoxWithButton),
                new PropertyMetadata(true, (o, args) => { }));
        
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("ActionButton") is Button actionButton)
            {
                actionButton.Click += (sender, e) => ButtonClick?.Invoke(sender, e);
            }
        }
    }
}
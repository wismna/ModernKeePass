using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModernKeePass.Controls
{
    public class TextBoxWithButton : TextBox
    {
        public string ButtonSymbol
        {
            get { return (string)GetValue(ButtonSymbolProperty); }
            set { SetValue(ButtonSymbolProperty, value); }
        }
        public static readonly DependencyProperty ButtonSymbolProperty =
            DependencyProperty.Register(
                "ButtonSymbol",
                typeof(string),
                typeof(TextBoxWithButton),
                new PropertyMetadata("&#xE107;", (o, args) => { }));
        public event EventHandler<RoutedEventArgs> ButtonClick;

        public string ButtonTooltip
        {
            get { return (string)GetValue(ButtonTooltipProperty); }
            set { SetValue(ButtonTooltipProperty, value); }
        }
        public static readonly DependencyProperty ButtonTooltipProperty =
            DependencyProperty.Register(
                "ButtonTooltip",
                typeof(string),
                typeof(TextBoxWithButton),
                new PropertyMetadata(string.Empty, (o, args) => { }));

        public bool IsButtonEnabled
        {
            get { return (bool)GetValue(IsButtonEnabledProperty); }
            set { SetValue(IsButtonEnabledProperty, value); }
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
            var actionButton = GetTemplateChild("ActionButton") as Button;
            if (actionButton != null)
            {
                actionButton.Click += (sender, e) => ButtonClick?.Invoke(sender, e);
            }
        }
    }
}
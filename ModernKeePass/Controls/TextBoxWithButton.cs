using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModernKeePass.Controls
{
    public class TextBoxWithButton : TextBox
    {
        /*public Symbol ButtonSymbol
        {
            get { return (Symbol)GetValue(ButtonSymbolProperty); }
            set { SetValue(ButtonSymbolProperty, value); }
        }
        public static readonly DependencyProperty ButtonSymbolProperty =
            DependencyProperty.Register(
                "ButtonSymbol",
                typeof(Symbol),
                typeof(TextBoxWithButton),
                new PropertyMetadata(Symbol.Delete, (o, args) => { }));*/

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
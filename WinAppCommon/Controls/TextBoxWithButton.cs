using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModernKeePass.Controls
{
    public class TextBoxWithButton : SearchBox
    {
        public event EventHandler<RoutedEventArgs> ButtonClick;

        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register(
                nameof(ButtonContent),
                typeof(string),
                typeof(TextBoxWithButton),
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
                typeof(TextBoxWithButton),
                new PropertyMetadata(string.Empty, (o, args) => { }));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(TextBoxWithButton),
                new PropertyMetadata(string.Empty, (o, args) => { }));
        
        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }
        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(
                nameof(ButtonCommand),
                typeof(ICommand),
                typeof(TextBoxWithButton),
                new PropertyMetadata(null, (o, args) => { }));

        public string ButtonCommandParameter
        {
            get { return (string)GetValue(ButtonCommandParameterProperty); }
            set { SetValue(ButtonCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty ButtonCommandParameterProperty =
            DependencyProperty.Register(
                nameof(ButtonCommandParameter),
                typeof(string),
                typeof(TextBoxWithButton),
                new PropertyMetadata(null, (o, args) => { }));

        public TextBoxWithButton()
        {
            DefaultStyleKey = typeof(TextBoxWithButton);
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
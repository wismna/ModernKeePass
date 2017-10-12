using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ModernKeePass.Controls
{
    public class TextBoxWithButton : TextBox
    {
        public event EventHandler<RoutedEventArgs> GotoClick;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var extraButton = GetTemplateChild("GotoButton") as Button;
            if (extraButton != null)
            {
                extraButton.Click += (sender, e) => GotoClick?.Invoke(sender, e);
            }
        }
    }
}
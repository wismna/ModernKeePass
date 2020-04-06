using System;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using ModernKeePass.Common;

namespace ModernKeePass.Actions
{
    public class NavigateToUrlAction : DependencyObject, IAction
    {
        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(NavigateToUrlAction), new PropertyMetadata(string.Empty));

        public object Execute(object sender, object parameter)
        {
            try
            {
                var uri = new Uri(Url);
                return Windows.System.Launcher.LaunchUriAsync(uri).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageDialogHelper.ShowErrorDialog(ex).GetAwaiter();
                return false;
            }
        }
    }
}

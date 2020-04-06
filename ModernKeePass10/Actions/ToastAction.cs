using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using ModernKeePass.Common;

namespace ModernKeePass.Actions
{
    public class ToastAction : DependencyObject, IAction
    {
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ToastAction), new PropertyMetadata(string.Empty));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ToastAction), new PropertyMetadata(string.Empty));

        public object Execute(object sender, object parameter)
        {
            ToastNotificationHelper.ShowGenericToast(Title, Message);
            return null;
        }
    }
}
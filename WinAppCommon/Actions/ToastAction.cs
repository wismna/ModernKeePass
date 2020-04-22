using Windows.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Interactivity;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Actions
{
    public class ToastAction : DependencyObject, IAction
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ToastAction), new PropertyMetadata(string.Empty));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ToastAction), new PropertyMetadata(string.Empty));

        public object Execute(object sender, object parameter)
        {
            var notification = App.Services.GetRequiredService<INotificationService>();
            notification.Show(Title, Message);
            return null;
        }
    }
}
using System;
using Windows.UI.Notifications;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Infrastructure.UWP
{
    public class ToastNotificationService: INotificationService
    {
        public void Show(string title, string text)
        {
            var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var toastElements = notificationXml.GetElementsByTagName("text");
            toastElements[0].AppendChild(notificationXml.CreateTextNode(title));
            toastElements[1].AppendChild(notificationXml.CreateTextNode(text));

            var toast = new ToastNotification(notificationXml)
            {
                ExpirationTime = DateTime.Now.AddSeconds(5)
            };
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
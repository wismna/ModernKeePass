using System;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Common
{
    public static class ToastNotificationHelper
    {
        public static void ShowMovedToast(IVmEntity entity, string action, string text)
        {
            var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var toastElements = notificationXml.GetElementsByTagName("text");
            toastElements[0].AppendChild(notificationXml.CreateTextNode($"{action} {entity.Title}"));
            toastElements[1].AppendChild(notificationXml.CreateTextNode(text));
            var toastNode = notificationXml.SelectSingleNode("/toast");

            // This is useful only for Windows 10 UWP
            var launch = new JsonObject
            {
                {"entityType", JsonValue.CreateStringValue(entity.GetType().Name)},
                {"entityId", JsonValue.CreateStringValue(entity.Id)}
            };
            ((XmlElement)toastNode)?.SetAttribute("launch", launch.Stringify());

            var toast = new ToastNotification(notificationXml)
            {
                ExpirationTime = DateTime.Now.AddSeconds(5)
            };
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public static void ShowGenericToast(string title, string text)
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

        public static void ShowErrorToast(Exception exception)
        {
            ShowGenericToast(exception.Source, exception.Message);
        }
    }
}

using System;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using ModernKeePass.Interfaces;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Services
{
    public static class ToastNotificationService
    {
        public static void ShowMovedToast(IPwEntity entity, string action, string text)
        {
            var entityType = entity is GroupVm ? "Group" : "Entry";
            var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var toastElements = notificationXml.GetElementsByTagName("text");
            toastElements[0].AppendChild(notificationXml.CreateTextNode($"{action} {entityType} {entity.Name}"));
            toastElements[1].AppendChild(notificationXml.CreateTextNode(text));
            var toastNode = notificationXml.SelectSingleNode("/toast");

            var launch = new JsonObject
            {
                {"entityType", JsonValue.CreateStringValue(entityType)},
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
    }
}

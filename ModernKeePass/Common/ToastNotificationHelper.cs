﻿using System;
using Windows.Data.Json;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using ModernKeePass.Interfaces;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Common
{
    public static class ToastNotificationHelper
    {
        public static void ShowMovedToast(IPwEntity entity, string action, string text)
        {
            var app = (App)Application.Current;
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
        
    }
}

using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Microsoft.QueryStringDotNET;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Common
{
    public static class ToastNotificationHelper
    {
        public static /*async*/ void ShowUndoToast(string entityType, IPwEntity entity)
        {
            // This is for Windows 10
            // Construct the visuals of the toast
            /*var visual = new ToastVisual
            {
                BindingGeneric = new ToastBindingGeneric
                {
                    Children =
                    {
                        new AdaptiveText
                        {
                            Text = $"{entityType} {entity.Name} deleted."
                        }
                    }
                }
            };
            
            // Construct the actions for the toast (inputs and buttons)
            var actions = new ToastActionsCustom
            {
                Buttons =
                {
                    new ToastButton("Undo", new QueryString
                    {
                        { "action", "undo" },
                        { "entityType", entityType },
                        { "entityId", entity.Id }

                    }.ToString())
                }
            };

            // Now we can construct the final toast content
            var toastContent = new ToastContent
            {
                Visual = visual,
                Actions = actions,
                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "undo" },
                    { "entityType", "group" },
                    { "entityId", entity.Id }

                }.ToString()
            };

            // And create the toast notification
            var toastXml = new XmlDocument();
            toastXml.LoadXml(toastContent.GetContent());
            
            var toast = new ToastNotification(toastXml) {ExpirationTime = DateTime.Now.AddSeconds(5)};
            toast.Dismissed += Toast_Dismissed;
            */
            var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var toastElements = notificationXml.GetElementsByTagName("text");
            toastElements[0].AppendChild(notificationXml.CreateTextNode($"{entityType} {entity.Name} deleted"));
            toastElements[1].AppendChild(notificationXml.CreateTextNode("Click me to undo"));
            var toastNode = notificationXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode)?.SetAttribute("launch", new QueryString
                {
                    { "entityType", entityType },
                    { "entityId", entity.Id }

                }.ToString());

            var toast = new ToastNotification(notificationXml)
            {
                ExpirationTime = DateTime.Now.AddSeconds(5)
            };
            toast.Dismissed += Toast_Dismissed;
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private static void Toast_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            var toastNode = sender.Content.SelectSingleNode("/toast");
            var launchArguments = QueryString.Parse(((XmlElement)toastNode)?.GetAttribute("launch"));
            var app = (App)Application.Current;
            var entity = app.PendingDeleteEntities[launchArguments["entityId"]];
            app.PendingDeleteEntities.Remove(launchArguments["entityId"]);
            entity.CommitDelete();
        }
    }
}

using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ModernKeePass.Common
{
    public static class MessageDialogHelper
    {
        public static async Task ShowActionDialog(string title, string contentText, string actionButtonText, string cancelButtonText, UICommandInvokedHandler actionCommand, UICommandInvokedHandler cancelCommand)
        {
            // Create the message dialog and set its content
            var messageDialog = CreateBasicDialog(title, contentText, cancelButtonText, cancelCommand);

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(actionButtonText, actionCommand));
            
            // Show the message dialog
            await messageDialog.ShowAsync();
        }
        
        public static async Task ShowErrorDialog(Exception exception)
        {
            if (exception == null) return;
            // Create the message dialog and set its content
            var messageDialog = CreateBasicDialog(exception.Message, exception.StackTrace, "OK");
            
            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        public static async Task ShowNotificationDialog(string title, string message)
        {
            var dialog = CreateBasicDialog(title, message, "OK");

            // Show the message dialog
            await dialog.ShowAsync();
        }

        private static MessageDialog CreateBasicDialog(string title, string message, string dismissActionText, UICommandInvokedHandler cancelCommand = null)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog(message, title);

            // Add commands and set their callbacks
            messageDialog.Commands.Add(new UICommand(dismissActionText, cancelCommand));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            return messageDialog;
        }
    }
}

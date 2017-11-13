using System;
using Windows.UI.Popups;

namespace ModernKeePass.Common
{
    public static class MessageDialogHelper
    {
        public static async void ShowDeleteConfirmationDialog(string actionText, string contentText, UICommandInvokedHandler action)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog(contentText);

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(actionText, action));
            messageDialog.Commands.Add(new UICommand("Cancel"));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        public static async void ShowErrorDialog(Exception exception)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog(exception.Message, "Error occured");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand("OK"));
            
            // Show the message dialog
            await messageDialog.ShowAsync();
        }
    }
}

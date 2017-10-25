using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Interfaces;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Common
{
    public static class MessageDialogHelper
    {
        public static async void ShowDeleteConfirmationDialog(string text, IPwEntity model, Frame backFrame)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog(text);

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand("Delete", delete =>
            {
                ToastNotificationHelper.ShowUndoToast(model);
                model.MarkForDelete();
                if (backFrame.CanGoBack) backFrame.GoBack();
            }));
            messageDialog.Commands.Add(new UICommand("Cancel"));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }
    }
}

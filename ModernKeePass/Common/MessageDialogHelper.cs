using System;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using ModernKeePass.Exceptions;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Common
{
    public static class MessageDialogHelper
    {
        // TODO: include resources
        public static async void ShowActionDialog(string title, string contentText, string actionButtonText, string cancelButtonText, UICommandInvokedHandler actionCommand, UICommandInvokedHandler cancelCommand)
        {
            // Create the message dialog and set its content
            var messageDialog = CreateBasicDialog(title, contentText, cancelButtonText, cancelCommand);

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(actionButtonText, actionCommand));
            
            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        public static void SaveErrorDialog(SaveException exception, IDatabaseService database)
        {
            ShowActionDialog("Save error", exception.InnerException.Message, "Save as", "Discard", async command => 
            {
                var savePicker = new FileSavePicker
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                    SuggestedFileName = $"{database.DatabaseFile.DisplayName} - copy"
                };
                savePicker.FileTypeChoices.Add("KeePass 2.x database", new List<string> { ".kdbx" });

                var file = await savePicker.PickSaveFileAsync();
                if (file != null) database.Save(file);
            }, null);
        }

        public static void SaveUnchangedDialog(DatabaseOpenedException exception, IDatabaseService database)
        {
            ShowActionDialog("Opened database", $"Database {database.Name} is currently opened. What to you wish to do?", "Save changes", "Discard", command => 
            {
                database.Save();
                database.Close();
            },
            command =>
            {
                database.Close();
            });
        }

        public static async void ShowErrorDialog(Exception exception)
        {
            if (exception == null) return;
            // Create the message dialog and set its content
            var messageDialog = CreateBasicDialog("Error occured", exception.Message, "OK");
            
            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        public static async void ShowNotificationDialog(string title, string message)
        {
            var dialog = CreateBasicDialog(title, message, "OK");

            // Show the message dialog
            await dialog.ShowAsync();
        }

        private static MessageDialog CreateBasicDialog(string title, string message, string dismissActionText, UICommandInvokedHandler cancelCommand = null)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog(message, title);

            // Add commands and set their callbacks; 
            messageDialog.Commands.Add(new UICommand(dismissActionText, cancelCommand));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            return messageDialog;
        }
    }
}

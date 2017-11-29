using System;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using ModernKeePass.Exceptions;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Services
{
    public static class MessageDialogService
    {
        public static async void ShowActionDialog(string title, string contentText, string actionButtonText, string cancelButtonText, UICommandInvokedHandler action)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog(contentText, title);

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(actionButtonText, action));
            messageDialog.Commands.Add(new UICommand(cancelButtonText));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        public static void SaveErrorDialog(SaveException exception, IDatabase database)
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
            });
        }

        public static async void ShowErrorDialog(Exception exception)
        {
            if (exception == null) return;
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog(exception.Message, "Error occured");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand("OK"));
            
            // Show the message dialog
            await messageDialog.ShowAsync();
        }
    }
}

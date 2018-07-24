using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.HockeyApp;
using ModernKeePass.Common;
using ModernKeePass.Exceptions;
using ModernKeePass.Services;
using ModernKeePass.Views;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace ModernKeePass
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
#if DEBUG
            HockeyClient.Current.Configure("2fe83672887b4910b9de93a4398d0f8f");
#else
			HockeyClient.Current.Configure("9eb5fbb79b484fbd8daf04635e975c84");
#endif
            InitializeComponent();
            Suspending += OnSuspending;
            Resuming += OnResuming;
            UnhandledException += OnUnhandledException;
        }

        #region Event Handlers

        private async void OnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            // Save the argument exception because it's cleared on first access
            var exception = unhandledExceptionEventArgs.Exception;
            var realException =
                exception is TargetInvocationException &&
                exception.InnerException != null
                    ? exception.InnerException
                    : exception;

            var database = DatabaseService.Instance;
            var resource = new ResourcesService();
            if (realException is SaveException)
            {
                unhandledExceptionEventArgs.Handled = true;
                await MessageDialogHelper.ShowActionDialog(resource.GetResourceValue("MessageDialogSaveErrorTitle"),
                    realException.InnerException.Message,
                    resource.GetResourceValue("MessageDialogSaveErrorButtonSaveAs"),
                    resource.GetResourceValue("MessageDialogSaveErrorButtonDiscard"), 
                    async command =>
                    {
                        var savePicker = new FileSavePicker
                        {
                            SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                            SuggestedFileName = $"{database.Name} - copy"
                        };
                        savePicker.FileTypeChoices.Add(resource.GetResourceValue("MessageDialogSaveErrorFileTypeDesc"),
                            new List<string> {".kdbx"});

                        var file = await savePicker.PickSaveFileAsync();
                        if (file != null) database.Save(file);
                    }, null);
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            await OnLaunchOrActivated(args);
            await HockeyClient.Current.SendCrashesAsync(/* sendWithoutAsking: true */);
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await OnLaunchOrActivated(args);
        }

        private async Task OnLaunchOrActivated(IActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame {Language = Windows.Globalization.ApplicationLanguages.Languages[0]};
                // Set the default language

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Load state from previously terminated application
                    await SuspensionManager.RestoreAsync();
#if DEBUG
                    await MessageDialogHelper.ShowNotificationDialog("App terminated", "Windows or an error made the app terminate");
#endif
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            var lauchActivatedEventArgs = e as LaunchActivatedEventArgs;
            if (lauchActivatedEventArgs != null && rootFrame.Content == null)
                rootFrame.Navigate(typeof(MainPage), lauchActivatedEventArgs.Arguments);

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void OnResuming(object sender, object e)
        {
            var currentFrame = Window.Current.Content as Frame;
            var database = DatabaseService.Instance;

            try
            {
                database.ReOpen();
#if DEBUG
                ToastNotificationHelper.ShowGenericToast(database.Name, "Database reopened (changes were saved)");
#endif
            }
            catch (Exception)
            {
                currentFrame?.Navigate(typeof(MainPage));
#if DEBUG
                ToastNotificationHelper.ShowGenericToast("App resumed", "Nothing to do, no previous database opened");
#endif
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new NavigationException(e.SourcePageType);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            var database = DatabaseService.Instance;
            try
            {
                if (SettingsService.Instance.GetSetting("SaveSuspend", true)) database.Save();
                database.Close(false);
            }
            catch (Exception exception)
            {
                ToastNotificationHelper.ShowErrorToast(exception);
            }
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
        
        /// <summary>
        /// Invoked when application is launched from opening a file in Windows Explorer 
        /// </summary>
        /// <param name="args">Details about the file being opened</param>
        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            base.OnFileActivated(args);
            var rootFrame = new Frame();
            var file = args.Files[0] as StorageFile;
            rootFrame.Navigate(typeof(MainPage), file);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }
        
        #endregion
    }
}

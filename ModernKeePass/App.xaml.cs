using System;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
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
            HockeyClient.Current.Configure("2fe83672887b4910b9de93a4398d0f8f");
            InitializeComponent();
            Suspending += OnSuspending;
            Resuming += OnResuming;
            UnhandledException += OnUnhandledException;
        }


        #region Event Handlers

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            // Save the argument exception because it's cleared on first access
            var exception = unhandledExceptionEventArgs.Exception;
            var realException =
                exception is TargetInvocationException &&
                exception.InnerException != null
                    ? exception.InnerException
                    : exception;

            if (realException is SaveException)
            {
                unhandledExceptionEventArgs.Handled = true;
                MessageDialogHelper.SaveErrorDialog(realException as SaveException, DatabaseService.Instance);
            }
            else if (realException is DatabaseOpenedException)
            {
                unhandledExceptionEventArgs.Handled = true;
                MessageDialogHelper.SaveUnchangedDialog(realException as DatabaseOpenedException, DatabaseService.Instance);
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            OnLaunchOrActivated(e);
            await HockeyClient.Current.SendCrashesAsync(/* sendWithoutAsking: true */);
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            OnLaunchOrActivated(args);
        }

        private void OnLaunchOrActivated(IActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //DebugSettings.EnableFrameRateCounter = true;
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
                    //TODO: Load state from previously terminated application
#if DEBUG
                    MessageDialogHelper.ShowNotificationDialog("App terminated", "Windows or an error made the app terminate");
#endif
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e is LaunchActivatedEventArgs)
            {
                var lauchActivatedEventArgs = (LaunchActivatedEventArgs) e;
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), lauchActivatedEventArgs.Arguments);
                }
                /*else
                {
                    // App is "launched" via the Toast Activation event
                    UndoEntityDelete(lauchActivatedEventArgs.Arguments);
                }*/
            }
            // This is only available on Windows 10...
            /*else if (e is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = e as ToastNotificationActivatedEventArgs;

                // Parse the query string (using QueryString.NET)
                UndoEntityDelete(QueryString.Parse(toastActivationArgs.Argument));
            }*/
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async void OnResuming(object sender, object e)
        {
            var currentFrame = Window.Current.Content as Frame;
            var database = DatabaseService.Instance;
            if (database.DatabaseFile == null)
            {
#if DEBUG
                ToastNotificationHelper.ShowGenericToast("App suspended", "Nothing to do, no previous database opened");
#endif
                return;
            }
            try
            {
                if (database.CompositeKey != null) await database.ReOpen();
            }
            catch (Exception ex)
            {
                currentFrame?.Navigate(typeof(MainPage));
#if DEBUG
                MessageDialogHelper.ShowErrorDialog(ex);
#endif
                ToastNotificationHelper.ShowGenericToast("App suspended", "Database was closed (changes were saved)");
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
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
                await database.Close(false);
            }
            catch (Exception exception)
            {
                ToastNotificationHelper.ShowErrorToast(exception);
            }
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
            DatabaseService.Instance.DatabaseFile = args.Files[0] as StorageFile;
            rootFrame.Navigate(typeof(MainPage), args);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }
        
        #endregion
    }
}

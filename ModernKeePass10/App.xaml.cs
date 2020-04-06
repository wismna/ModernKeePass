using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using UnhandledExceptionEventArgs = Windows.UI.Xaml.UnhandledExceptionEventArgs;
using Autofac;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using ModernKeePass.Common;
using ModernKeePass.Composition;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Views;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace ModernKeePass
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        private readonly IDatabaseService _databaseService;

        public static IContainer Container { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            AppCenter.Start("79d23520-a486-4f63-af81-8d90bf4e1bea", typeof(Analytics));

            InitializeComponent();
            Suspending += OnSuspending;
            Resuming += OnResuming;
            UnhandledException += OnUnhandledException;

            // Setup DI
            var builder = new ContainerBuilder();
            builder.RegisterModule<SharedCompositionRoot>();
            builder.RegisterModule<UwpCompositionRoot>();
            Container = builder.Build();

            _databaseService = Container.Resolve<IDatabaseService>();
        }

        #region Event Handlers

        // TODO: do something else here instead of showing dialog and handle save issues directly where it happens
        private async void OnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            // Save the argument exception because it's cleared on first access
            var exception = unhandledExceptionEventArgs.Exception;
            var realException =
                exception is TargetInvocationException &&
                exception.InnerException != null
                    ? exception.InnerException
                    : exception;

            var resource = Container.Resolve<IResourceService>();
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
                            SuggestedFileName = $"{_databaseService.Name} - copy"
                        };
                        savePicker.FileTypeChoices.Add(resource.GetResourceValue("MessageDialogSaveErrorFileTypeDesc"),
                            new List<string> {".kdbx"});

                        var file = await savePicker.PickSaveFileAsync();
                        var token = StorageApplicationPermissions.FutureAccessList.Add(file);
                        var fileInfo = new FileInfo
                        {
                            Path = token,
                            Name = file.DisplayName
                        };
                        await _databaseService.SaveAs(fileInfo);
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
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await OnLaunchOrActivated(args);
        }

        private async Task OnLaunchOrActivated(IActivatedEventArgs e)
        {
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
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

            if (e is LaunchActivatedEventArgs lauchActivatedEventArgs && rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage10), lauchActivatedEventArgs.Arguments);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void OnResuming(object sender, object e)
        {
            var currentFrame = Window.Current.Content as Frame;

            try
            {
                //_databaseService.ReOpen();
#if DEBUG
                ToastNotificationHelper.ShowGenericToast(_databaseService.Name, "Database reopened (changes were saved)");
#endif
            }
            catch (Exception)
            {
                currentFrame?.Navigate(typeof(MainPage10));
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
            var settings = Container.Resolve<ISettingsService>();
            try
            {
                // TODO: definitely do something about this to avoid DB corruption if app closes before save has completed
                if (settings.GetSetting("SaveSuspend", true)) await _databaseService.Save();
                _databaseService.Close();
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
            rootFrame.Navigate(typeof(MainPage10), args.Files[0] as StorageFile);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }
        
        #endregion
    }
}

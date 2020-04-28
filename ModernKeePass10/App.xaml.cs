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
using GalaSoft.MvvmLight.Views;
using MediatR;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Database.Queries.ReOpenDatabase;
using ModernKeePass.Common;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;
using ModernKeePass.Infrastructure;
using ModernKeePass.Views;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace ModernKeePass
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        private readonly IResourceProxy _resource;
        private readonly IMediator _mediator;
        private readonly ISettingsProxy _settings;
        private readonly INavigationService _navigation;
        private readonly IAppCenterService _appCenter;
        private readonly IDialogService _dialog;
        private readonly INotificationService _notification;

        public static IServiceProvider Services { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            // Setup DI
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddApplication();
            serviceCollection.AddInfrastructureCommon();
            serviceCollection.AddInfrastructureKeePass();
            serviceCollection.AddInfrastructureUwp();
            serviceCollection.AddWin10App();
            Services = serviceCollection.BuildServiceProvider();

            _mediator = Services.GetService<IMediator>();
            _resource = Services.GetService<IResourceProxy>();
            _settings = Services.GetService<ISettingsProxy>();
            _navigation = Services.GetService<INavigationService>();
            _dialog = Services.GetService<IDialogService>();
            _notification = Services.GetService<INotificationService>();

#if DEBUG
            AppCenter.Start("029ab91d-1e4b-4d4d-9661-5d438dd671a5",
                typeof(Analytics), typeof(Crashes));
#else
			AppCenter.Start("79d23520-a486-4f63-af81-8d90bf4e1bea", typeof(Analytics));
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

            if (realException is SaveException)
            {
                unhandledExceptionEventArgs.Handled = true;
                //_hockey.TrackException(realException);
                await _dialog.ShowMessage(realException.Message,
                    _resource.GetResourceValue("MessageDialogSaveErrorTitle"),
                    _resource.GetResourceValue("MessageDialogSaveErrorButtonSaveAs"),
                    _resource.GetResourceValue("MessageDialogSaveErrorButtonDiscard"),
                    async isOk =>
                    {
                        if (isOk)
                        {
                            var database = await _mediator.Send(new GetDatabaseQuery());
                            var savePicker = new FileSavePicker
                            {
                                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                                SuggestedFileName = $"{database.Name} - copy"
                            };
                            savePicker.FileTypeChoices.Add(
                                _resource.GetResourceValue("MessageDialogSaveErrorFileTypeDesc"),
                                new List<string> { ".kdbx" });

                            var file = await savePicker.PickSaveFileAsync().AsTask();
                            if (file != null)
                            {
                                var token = StorageApplicationPermissions.FutureAccessList.Add(file, file.Name);
                                await _mediator.Send(new SaveDatabaseCommand { FilePath = token });
                            }
                        }
                    });
            }
            else
            {
                await _dialog.ShowError(realException, realException.Message, "OK", () => { });
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
            //await _hockey.SendCrashesAsync(/* sendWithoutAsking: true */);
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await OnLaunchOrActivated(args);
        }

        private async Task OnLaunchOrActivated(IActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame { Language = Windows.Globalization.ApplicationLanguages.Languages[0] };
                // Set the default language

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Load state from previously terminated application
                    await SuspensionManager.RestoreAsync();
#if DEBUG
                    await _dialog.ShowMessage("Windows or an error made the app terminate", "App terminated");
#endif
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            var launchActivatedEventArgs = e as LaunchActivatedEventArgs;
            if (launchActivatedEventArgs != null && rootFrame.Content == null)
                _navigation.NavigateTo(Constants.Navigation.MainPage, launchActivatedEventArgs.Arguments);

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async void OnResuming(object sender, object e)
        {
            var currentFrame = Window.Current.Content as Frame;

            try
            {
                await _mediator.Send(new ReOpenDatabaseQuery());
#if DEBUG
                _notification.Show("App resumed", "Database reopened (changes were saved)");
#endif
            }
            catch (Exception)
            {
                currentFrame?.Navigate(typeof(MainPage10));
#if DEBUG
                _notification.Show("App resumed", "Nothing to do, no previous database opened");
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
            try
            {
                var database = await _mediator.Send(new GetDatabaseQuery());
                if (database.IsOpen)
                {
                    if (database.Size < Constants.File.OneMegaByte && database.IsDirty &&
                        _settings.GetSetting(Constants.Settings.SaveSuspend, true))
                    {
                        await _mediator.Send(new SaveDatabaseCommand()).ConfigureAwait(false);
                    }

                    await _mediator.Send(new CloseDatabaseCommand()).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                _notification.Show(exception.Source, exception.Message);
            }
            finally
            {
                await SuspensionManager.SaveAsync().ConfigureAwait(false);
                deferral.Complete();
            }
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

            Window.Current.Content = rootFrame;

            if (file != null)
            {
                // TODO: use service
                var token = StorageApplicationPermissions.MostRecentlyUsedList.Add(file, file.Path);
                var fileInfo = new FileInfo
                {
                    Id = token,
                    Name = file.DisplayName,
                    Path = file.Path
                };
                _navigation.NavigateTo(Constants.Navigation.MainPage, fileInfo);
            }
            else
            {
                _navigation.NavigateTo(Constants.Navigation.MainPage);
            }

            Window.Current.Activate();
        }

        #endregion
    }
}

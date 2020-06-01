using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Interactivity;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Common;

namespace ModernKeePass.Actions
{
    public class ClipboardAction : DependencyObject, IAction
    {
        private bool _isWindowActivated = true;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(ClipboardAction), new PropertyMetadata(string.Empty));
        public bool IsProtected
        {
            get { return (bool)GetValue(IsProtectedProperty); }
            set { SetValue(IsProtectedProperty, value); }
        }

        public static readonly DependencyProperty IsProtectedProperty =
            DependencyProperty.Register(nameof(IsProtected), typeof(bool), typeof(ClipboardAction), new PropertyMetadata(false));

        public object Execute(object sender, object parameter)
        {
            if (string.IsNullOrEmpty(Text)) return null;

            var settings = App.Services.GetRequiredService<ISettingsProxy>();
            var cryptography = App.Services.GetRequiredService<ICryptographyClient>();

            CoreWindow.GetForCurrentThread().Activated += ClipboardAction_Activated;

            var dispatcher = new DispatcherTimer {Interval = TimeSpan.FromSeconds(settings.GetSetting(Constants.Settings.ClipboardTimeout, 10))};
            dispatcher.Tick += Dispatcher_Tick;

            var dataPackage = new DataPackage { RequestedOperation = DataPackageOperation.Copy };
            dataPackage.SetText(IsProtected ? cryptography.UnProtect(Text).GetAwaiter().GetResult() : Text);
            Clipboard.SetContent(dataPackage);
            Clipboard.Flush();
            dispatcher.Start();

            return null;
        }

        private void ClipboardAction_Activated(CoreWindow sender, WindowActivatedEventArgs args)
        {
            _isWindowActivated = args.WindowActivationState != CoreWindowActivationState.Deactivated;
        }

        private void Dispatcher_Tick(object sender, object e)
        {
            if (_isWindowActivated) Clipboard.SetContent(null);
        }
    }
}

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Interactivity;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Common;

namespace ModernKeePass.Actions
{
    public class ClipboardAction : DependencyObject, IAction
    {
        private DispatcherTimer _dispatcher;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ClipboardAction), new PropertyMetadata(string.Empty));

        public object Execute(object sender, object parameter)
        {
            if (string.IsNullOrEmpty(Text)) return null;

            var settings = App.Services.GetRequiredService<ISettingsProxy>();

            _dispatcher = new DispatcherTimer {Interval = TimeSpan.FromSeconds(settings.GetSetting(Constants.Settings.ClipboardTimeout, 10))};
            _dispatcher.Tick += Dispatcher_Tick;

            var dataPackage = new DataPackage { RequestedOperation = DataPackageOperation.Copy };
            dataPackage.SetText(Text);
            Clipboard.SetContent(dataPackage);
            _dispatcher.Start();

            return null;
        }

        private void Dispatcher_Tick(object sender, object e)
        {
            Clipboard.SetContent(null);
            _dispatcher.Stop();
        }
    }
}

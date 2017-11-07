using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;

namespace ModernKeePass.Actions
{
    public class ClipboardAction : DependencyObject, IAction
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ClipboardAction), new PropertyMetadata(string.Empty));

        public object Execute(object sender, object parameter)
        {
            var dataPackage = new DataPackage { RequestedOperation = DataPackageOperation.Copy };
            dataPackage.SetText(Text);
            Clipboard.SetContent(dataPackage);
            return null;
        }
    }
}

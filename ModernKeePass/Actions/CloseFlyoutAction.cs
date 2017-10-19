using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace ModernKeePass.Actions
{
    public class CloseFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var flyout = sender as Flyout;
            flyout?.Hide();

            return null;
        }
    }
}

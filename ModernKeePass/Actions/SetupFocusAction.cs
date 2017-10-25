using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace ModernKeePass.Actions
{
    public class SetupFocusAction : DependencyObject, IAction
    {
        public Control TargetObject
        {
            get { return (Control)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(Control), typeof(SetupFocusAction), new PropertyMetadata(null));

        public object Execute(object sender, object parameter)
        {
            return Task.Factory.StartNew(
                () => Dispatcher.RunAsync(CoreDispatcherPriority.Low,
                    () => TargetObject?.Focus(FocusState.Programmatic)));
        }
    }
}
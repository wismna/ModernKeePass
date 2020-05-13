// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.System;
using Windows.UI.Xaml.Input;

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsGeneralPage
    {
        public SettingsGeneralPage()
        {
            InitializeComponent();
        }

        private void UIElement_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if ((e.Key < VirtualKey.NumberPad0 || e.Key > VirtualKey.NumberPad9) & (e.Key < VirtualKey.Number0 || e.Key > VirtualKey.Number9))
            {
                e.Handled = true;
            }
        }
    }
}

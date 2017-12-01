// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using ModernKeePass.Common;
using ModernKeePass.Events;
using ModernKeePass.Services;

namespace ModernKeePass.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsSecurityPage
    {
        public SettingsSecurityPage()
        {
            InitializeComponent();
        }

        private void CompositeKeyUserControl_OnValidationChecked(object sender, PasswordEventArgs e)
        {
            ToastNotificationHelper.ShowGenericToast("Composite key", "Database successfully updated.");
        }
    }
}

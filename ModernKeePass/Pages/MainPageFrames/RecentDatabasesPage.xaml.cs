// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

using System;
using Windows.UI.Xaml;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Pages
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class RecentDatabasesPage
    {

        public RecentVm Model => (RecentVm)DataContext;
        public RecentDatabasesPage()
        {
            InitializeComponent();
        }

        private void OpenDatabaseUserControl_OnValidationChecking(object sender, EventArgs e)
        {
            var app = (App)Application.Current;
            app.Database.DatabaseFile = ((RecentItemVm)Model.SelectedItem).DatabaseFile;
        }
    }
}

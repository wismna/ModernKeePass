﻿// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

using ModernKeePass.ViewModels;

namespace ModernKeePass.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class RecentDatabasesPage
    {
        private RecentViewModel ViewModel { get; }

        public RecentDatabasesPage()
        {
            InitializeComponent();
            ViewModel = new RecentViewModel();
        }
    }
}

﻿// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.Views.SettingsPageFrames
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsSavePage
    {
        private SettingsSaveViewModel ViewModel { get; }
        public SettingsSavePage()
        {
            InitializeComponent();
            ViewModel = new SettingsSaveViewModel();
        }
    }
}

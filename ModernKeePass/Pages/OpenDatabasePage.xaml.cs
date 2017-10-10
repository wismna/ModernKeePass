﻿using System;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Events;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenDatabasePage : Page
    {
        private Frame _mainFrame;

        public OpenDatabasePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainFrame = e.Parameter as Frame;
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var picker =
                new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
            picker.FileTypeFilter.Add(".kdbx");
            
            var viewModel = DataContext as OpenVm;
            // Application now has read/write access to the picked file
            viewModel.OpenFile(await picker.PickSingleFileAsync());
        }

        private void PasswordUserControl_PasswordChecked(object sender, PasswordEventArgs e)
        {
            _mainFrame.Navigate(typeof(GroupDetailPage), e.RootGroup);
        }
    }
}

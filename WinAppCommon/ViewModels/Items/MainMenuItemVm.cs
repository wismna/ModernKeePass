﻿using Windows.UI.Xaml.Controls;

namespace ModernKeePass.ViewModels.ListItems
{
    public class MainMenuItemVm: ListMenuItemVm
    {
        public object Parameter { get; set; }
        public Frame Destination { get; set; }
    }
}

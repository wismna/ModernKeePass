using System;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels.ListItems
{
    public class ListMenuItemVm : ObservableObject, IIsEnabled, ISelectableModel
    {
        private bool _isSelected;

        public string Title { get; set; }
        
        public string Group { get; set; } = "_";
        public Type PageType { get; set; }
        public Symbol SymbolIcon { get; set; }
        public bool IsEnabled { get; set; } = true;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(() => IsSelected, ref _isSelected, value); }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}

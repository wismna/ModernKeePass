using System;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class ListMenuItemVm : NotifyPropertyChangedBase, IIsEnabled, ISelectableModel
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
            set { SetProperty(ref _isSelected, value); }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}

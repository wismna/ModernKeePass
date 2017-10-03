using System.Linq;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels
{
    public class MainVm : NotifyPropertyChangedBase
    {
        private IOrderedEnumerable<IGrouping<int, MainMenuItemVm>> _mainMenuItems;

        public IOrderedEnumerable<IGrouping<int, MainMenuItemVm>> MainMenuItems
        {
            get { return _mainMenuItems; }
            set { SetProperty(ref _mainMenuItems, value); }
        }
    }
}

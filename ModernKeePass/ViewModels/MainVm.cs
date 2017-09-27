using System.Collections.ObjectModel;
using ModernKeePass.Models;

namespace ModernKeePass.ViewModels
{
    public class MainVm
    {
        public ObservableCollection<MainMenuItem> MainMenuItems { get; set; }
    }
}

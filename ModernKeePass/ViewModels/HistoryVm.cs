using System.Collections.Generic;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class HistoryVm: IHasSelectableObject
    {
        public IEnumerable<IPwEntity> History { get; }

        public HistoryVm() { }

        public HistoryVm(IEnumerable<IPwEntity> history)
        {
            History = history;
        }

        public ISelectableModel SelectedItem { get; set; }
    }
}
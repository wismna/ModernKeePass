using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernKeePass.ViewModels
{
    public class GroupVm
    {
        public List<EntryVm> Entries { get; set; }
        public string Name { get; set; }
    }
}

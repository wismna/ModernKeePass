using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernKeePassLib;

namespace ModernKeePass.ViewModels
{
    public class EntryVm
    {
        public string Title { get; private set; }
        public string UserName { get; private set; }
        public string URL { get; private set; }
        public string Notes { get; private set; }

        public EntryVm() { }
        public EntryVm(PwEntry entry)
        {
            Title = entry.Strings.GetSafe(PwDefs.TitleField).ReadString();
            UserName = entry.Strings.GetSafe(PwDefs.UserNameField).ReadString();
            URL = entry.Strings.GetSafe(PwDefs.UrlField).ReadString();
            Notes = entry.Strings.GetSafe(PwDefs.NotesField).ReadString();
        }
    }
}

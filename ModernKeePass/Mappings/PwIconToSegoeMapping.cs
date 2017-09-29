using Windows.UI.Xaml.Controls;
using ModernKeePassLib;

namespace ModernKeePass.Mappings
{
    public static class PwIconToSegoeMapping
    {
        public static Symbol GetSymbolFromIcon(PwIcon icon)
        {
            switch (icon)
            {
                case PwIcon.Key: return Symbol.Permissions;
                case PwIcon.World: return Symbol.World;
                case PwIcon.Warning: return Symbol.Important;
                case PwIcon.WorldComputer:
                case PwIcon.DriveWindows:
                case PwIcon.NetworkServer: return Symbol.MapDrive;
                //case PwIcon.MarkedDirectory: return Symbol.;
                case PwIcon.UserCommunication: return Symbol.ContactInfo;
                //case PwIcon.Parts: return Symbol.;
                case PwIcon.Notepad: return Symbol.Document;
                //case PwIcon.WorldScoket: return Symbol.;
                case PwIcon.Identity: return Symbol.Contact2;
                //case PwIcon.PaperReady: return Symbol.;
                case PwIcon.Digicam: return Symbol.Camera;
                case PwIcon.IRCommunication: return Symbol.View;
                case PwIcon.Energy: return Symbol.ZeroBars;
                case PwIcon.Scanner: return Symbol.Scan;
                case PwIcon.CDRom: return Symbol.Rotate;
                case PwIcon.Monitor: return Symbol.Caption;
                case PwIcon.EMailBox: 
                case PwIcon.EMail: return Symbol.Mail;
                case PwIcon.Configuration: return Symbol.Setting;
                case PwIcon.ClipboardReady: return Symbol.Paste;
                case PwIcon.PaperNew: return Symbol.Page2;
                //case PwIcon.Screen: return Symbol.Document;
                case PwIcon.EnergyCareful: return Symbol.FourBars;
                case PwIcon.Disk: return Symbol.Save;
                //case PwIcon.Drive: return Symbol.;
                //case PwIcon.PaperQ: return Symbol.;
                //case PwIcon.TerminalEncrypted: return Symbol.;
                //case PwIcon.Console: return Symbol.;
                //case PwIcon.Printer: return Symbol.;
                case PwIcon.ProgramIcons: return Symbol.GoToStart;
                //case PwIcon.Run: return Symbol.;
                case PwIcon.Settings:
                case PwIcon.Tool: return Symbol.Repair;
                //case PwIcon.Archive: return Symbol.;
                case PwIcon.Count: return Symbol.MapDrive;
                case PwIcon.Clock: return Symbol.Clock;
                case PwIcon.EMailSearch: return Symbol.Find;
                case PwIcon.PaperFlag: return Symbol.Flag;
                //case PwIcon.Memory: return Symbol.;
                case PwIcon.TrashBin: return Symbol.Delete;
                case PwIcon.Expired: return Symbol.Cancel;
                case PwIcon.Info: return Symbol.Help;
                //case PwIcon.Package: return Symbol.;
                case PwIcon.Folder:
                case PwIcon.FolderOpen: 
                case PwIcon.FolderPackage: return Symbol.Folder;
                //case PwIcon.LockOpen: return Symbol.;
                case PwIcon.PaperLocked: return Symbol.ProtectedDocument;
                case PwIcon.Checked: return Symbol.Accept;
                case PwIcon.Pen: return Symbol.Edit;
                case PwIcon.Thumbnail: return Symbol.BrowsePhotos;
                case PwIcon.Book: return Symbol.Library;
                case PwIcon.List: return Symbol.List;
                case PwIcon.UserKey: return Symbol.ContactPresence;
                case PwIcon.Home: return Symbol.Home;
                case PwIcon.Star: return Symbol.OutlineStar;
                //case PwIcon.Tux: return Symbol.;
                //case PwIcon.Feather: return Symbol.;
                //case PwIcon.Apple: return Symbol.;
                //case PwIcon.Wiki: return Symbol.;
                //case PwIcon.Money: return Symbol.;
                case PwIcon.Certificate: return Symbol.PreviewLink;
                case PwIcon.BlackBerry: return Symbol.CellPhone;
                default: return Symbol.More;
            }
        }
    }
}

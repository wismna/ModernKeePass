using ModernKeePass.Domain.Enums;
using ModernKeePassLib;

namespace ModernKeePass.Infrastructure.KeePass
{
    public static class IconMapper
    {
        public static Icon MapPwIconToIcon(PwIcon value)
        {
            switch (value)
            {
                case PwIcon.Key: return Icon.Permissions;
                case PwIcon.WorldSocket:
                case PwIcon.World: return Icon.World;
                case PwIcon.Warning: return Icon.Important;
                case PwIcon.WorldComputer:
                case PwIcon.Drive:
                case PwIcon.DriveWindows:
                case PwIcon.NetworkServer: return Icon.MapDrive;
                case PwIcon.MarkedDirectory: return Icon.Map;
                case PwIcon.UserCommunication: return Icon.ContactInfo;
                case PwIcon.Parts: return Icon.ViewAll;
                case PwIcon.Notepad: return Icon.Document;
                case PwIcon.Identity: return Icon.Contact;
                case PwIcon.PaperReady: return Icon.SyncFolder;
                case PwIcon.Digicam: return Icon.Camera;
                case PwIcon.IRCommunication: return Icon.View;
                case PwIcon.Energy: return Icon.ZeroBars;
                case PwIcon.Scanner: return Icon.Scan;
                case PwIcon.CDRom: return Icon.Rotate;
                case PwIcon.Monitor: return Icon.Caption;
                case PwIcon.EMailBox:
                case PwIcon.EMail: return Icon.Mail;
                case PwIcon.Configuration: return Icon.Setting;
                case PwIcon.ClipboardReady: return Icon.Paste;
                case PwIcon.PaperNew: return Icon.Page;
                case PwIcon.Screen: return Icon.GoToStart;
                case PwIcon.EnergyCareful: return Icon.FourBars;
                case PwIcon.Disk: return Icon.Save;
                case PwIcon.Console: return Icon.SlideShow;
                case PwIcon.Printer: return Icon.Scan;
                case PwIcon.ProgramIcons: return Icon.GoToStart;
                case PwIcon.Settings:
                case PwIcon.Tool: return Icon.Repair;
                case PwIcon.Archive: return Icon.Crop;
                case PwIcon.Count: return Icon.Calculator;
                case PwIcon.Clock: return Icon.Clock;
                case PwIcon.EMailSearch: return Icon.Find;
                case PwIcon.PaperFlag: return Icon.Flag;
                case PwIcon.TrashBin: return Icon.Delete;
                case PwIcon.Expired: return Icon.ReportHacked;
                case PwIcon.Info: return Icon.Help;
                case PwIcon.Folder:
                case PwIcon.FolderOpen:
                case PwIcon.FolderPackage: return Icon.Folder;
                case PwIcon.PaperLocked: return Icon.ProtectedDocument;
                case PwIcon.Checked: return Icon.Accept;
                case PwIcon.Pen: return Icon.Edit;
                case PwIcon.Thumbnail: return Icon.BrowsePhotos;
                case PwIcon.Book: return Icon.Library;
                case PwIcon.List: return Icon.List;
                case PwIcon.UserKey: return Icon.ContactPresence;
                case PwIcon.Home: return Icon.Home;
                case PwIcon.Star: return Icon.OutlineStar;
                case PwIcon.Money: return Icon.Shop;
                case PwIcon.Certificate: return Icon.PreviewLink;
                case PwIcon.BlackBerry: return Icon.CellPhone;
                default: return Icon.Stop;
            }
        }

        public static PwIcon MapIconToPwIcon(Icon value)
        {
            switch (value)
            {
                case Icon.Delete: return PwIcon.TrashBin;
                case Icon.Edit: return PwIcon.Pen;
                case Icon.Save: return PwIcon.Disk;
                case Icon.Cancel: return PwIcon.Expired;
                case Icon.Accept: return PwIcon.Checked;
                case Icon.Home: return PwIcon.Home;
                case Icon.Camera: return PwIcon.Digicam;
                case Icon.Setting: return PwIcon.Configuration;
                case Icon.Mail: return PwIcon.EMail;
                case Icon.Find: return PwIcon.EMailSearch;
                case Icon.Help: return PwIcon.Info;
                case Icon.Clock: return PwIcon.Clock;
                case Icon.Crop: return PwIcon.Archive;
                case Icon.World: return PwIcon.World;
                case Icon.Flag: return PwIcon.PaperFlag;
                case Icon.PreviewLink: return PwIcon.Certificate;
                case Icon.Document: return PwIcon.Notepad;
                case Icon.ProtectedDocument: return PwIcon.PaperLocked;
                case Icon.ContactInfo: return PwIcon.UserCommunication;
                case Icon.ViewAll: return PwIcon.Parts;
                case Icon.Rotate: return PwIcon.CDRom;
                case Icon.List: return PwIcon.List;
                case Icon.Shop: return PwIcon.Money;
                case Icon.BrowsePhotos: return PwIcon.Thumbnail;
                case Icon.Caption: return PwIcon.Monitor;
                case Icon.Repair: return PwIcon.Tool;
                case Icon.Page: return PwIcon.PaperNew;
                case Icon.Paste: return PwIcon.ClipboardReady;
                case Icon.Important: return PwIcon.Warning;
                case Icon.SlideShow: return PwIcon.Console;
                case Icon.MapDrive: return PwIcon.NetworkServer;
                case Icon.ContactPresence: return PwIcon.UserKey;
                case Icon.Contact: return PwIcon.Identity;
                case Icon.Folder: return PwIcon.Folder;
                case Icon.View: return PwIcon.IRCommunication;
                case Icon.Permissions: return PwIcon.Key;
                case Icon.Map: return PwIcon.MarkedDirectory;
                case Icon.CellPhone: return PwIcon.BlackBerry;
                case Icon.OutlineStar: return PwIcon.Star;
                case Icon.Calculator: return PwIcon.Count;
                case Icon.Library: return PwIcon.Book;
                case Icon.SyncFolder: return PwIcon.PaperReady;
                case Icon.GoToStart: return PwIcon.Screen;
                case Icon.ZeroBars: return PwIcon.Energy;
                case Icon.FourBars: return PwIcon.EnergyCareful;
                case Icon.Scan: return PwIcon.Scanner;
                default: return PwIcon.Key;
            }
        }
    }
}

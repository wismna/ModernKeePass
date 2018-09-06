using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using ModernKeePassLib;

namespace ModernKeePass.Converters
{
    public class IntToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var icon = (PwIcon) value;
            switch (icon)
            {
                case PwIcon.Key: return Symbol.Permissions;
                case PwIcon.WorldSocket:
                case PwIcon.World: return Symbol.World;
                case PwIcon.Warning: return Symbol.Important;
                case PwIcon.WorldComputer:
                case PwIcon.Drive:
                case PwIcon.DriveWindows:
                case PwIcon.NetworkServer: return Symbol.MapDrive;
                case PwIcon.MarkedDirectory: return Symbol.Map;
                case PwIcon.UserCommunication: return Symbol.ContactInfo;
                case PwIcon.Parts: return Symbol.ViewAll;
                case PwIcon.Notepad: return Symbol.Document;
                case PwIcon.Identity: return Symbol.Contact2;
                case PwIcon.PaperReady: return Symbol.SyncFolder;
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
                case PwIcon.Screen: return Symbol.GoToStart;
                case PwIcon.EnergyCareful: return Symbol.FourBars;
                case PwIcon.Disk: return Symbol.Save;
                case PwIcon.Console: return Symbol.SlideShow;
                case PwIcon.Printer: return Symbol.Scan;
                case PwIcon.ProgramIcons: return Symbol.GoToStart;
                case PwIcon.Settings:
                case PwIcon.Tool: return Symbol.Repair;
                case PwIcon.Archive: return Symbol.Crop;
                case PwIcon.Count: return Symbol.Calculator;
                case PwIcon.Clock: return Symbol.Clock;
                case PwIcon.EMailSearch: return Symbol.Find;
                case PwIcon.PaperFlag: return Symbol.Flag;
                case PwIcon.TrashBin: return Symbol.Delete;
                case PwIcon.Expired: return Symbol.ReportHacked;
                case PwIcon.Info: return Symbol.Help;
                case PwIcon.Folder:
                case PwIcon.FolderOpen:
                case PwIcon.FolderPackage: return Symbol.Folder;
                case PwIcon.PaperLocked: return Symbol.ProtectedDocument;
                case PwIcon.Checked: return Symbol.Accept;
                case PwIcon.Pen: return Symbol.Edit;
                case PwIcon.Thumbnail: return Symbol.BrowsePhotos;
                case PwIcon.Book: return Symbol.Library;
                case PwIcon.List: return Symbol.List;
                case PwIcon.UserKey: return Symbol.ContactPresence;
                case PwIcon.Home: return Symbol.Home;
                case PwIcon.Star: return Symbol.OutlineStar;
                case PwIcon.Money: return Symbol.Shop;
                case PwIcon.Certificate: return Symbol.PreviewLink;
                case PwIcon.BlackBerry: return Symbol.CellPhone;
                default: return Symbol.Stop;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var symbol = (Symbol) value;
            var defaultIcon = (int?) parameter ?? -1;
            switch (symbol)
            {
                case Symbol.Delete: return (int)PwIcon.TrashBin;
                case Symbol.Edit: return (int) PwIcon.Pen;
                case Symbol.Save: return (int) PwIcon.Disk;
                case Symbol.Cancel: return (int) PwIcon.Expired;
                case Symbol.Accept: return (int) PwIcon.Checked;
                case Symbol.Home: return (int) PwIcon.Home;
                case Symbol.Camera: return (int) PwIcon.Digicam;
                case Symbol.Setting: return (int) PwIcon.Configuration;
                case Symbol.Mail: return (int) PwIcon.EMail;
                case Symbol.Find: return (int) PwIcon.EMailSearch;
                case Symbol.Help: return (int) PwIcon.Info;
                case Symbol.Clock: return (int) PwIcon.Clock;
                case Symbol.Crop: return (int) PwIcon.Archive;
                case Symbol.World: return (int) PwIcon.World;
                case Symbol.Flag: return (int) PwIcon.PaperFlag;
                case Symbol.PreviewLink: return (int) PwIcon.Certificate;
                case Symbol.Document: return (int) PwIcon.Notepad;
                case Symbol.ProtectedDocument: return (int) PwIcon.PaperLocked;
                case Symbol.ContactInfo: return (int) PwIcon.UserCommunication;
                case Symbol.ViewAll: return (int) PwIcon.Parts;
                case Symbol.Rotate: return (int) PwIcon.CDRom;
                case Symbol.List: return (int) PwIcon.List;
                case Symbol.Shop: return (int) PwIcon.Money;
                case Symbol.BrowsePhotos: return (int) PwIcon.Thumbnail;
                case Symbol.Caption: return (int) PwIcon.Monitor;
                case Symbol.Repair: return (int) PwIcon.Tool;
                case Symbol.Page2: return (int) PwIcon.PaperNew;
                case Symbol.Paste: return (int) PwIcon.ClipboardReady;
                case Symbol.Important: return (int) PwIcon.Warning;
                case Symbol.SlideShow: return (int) PwIcon.Console;
                case Symbol.MapDrive: return (int) PwIcon.NetworkServer;
                case Symbol.ContactPresence: return (int) PwIcon.UserKey;
                case Symbol.Contact2: return (int) PwIcon.Identity;
                case Symbol.Folder: return (int) PwIcon.Folder;
                case Symbol.View: return (int) PwIcon.IRCommunication;
                case Symbol.Permissions: return (int) PwIcon.Key;
                case Symbol.Map: return (int) PwIcon.MarkedDirectory;
                case Symbol.CellPhone: return (int) PwIcon.BlackBerry;
                case Symbol.OutlineStar: return (int) PwIcon.Star;
                case Symbol.Calculator: return (int) PwIcon.Count;
                case Symbol.Library: return (int) PwIcon.Book;
                case Symbol.SyncFolder: return (int) PwIcon.PaperReady;
                case Symbol.GoToStart: return (int) PwIcon.Screen;
                case Symbol.ZeroBars: return (int) PwIcon.Energy;
                case Symbol.FourBars: return (int) PwIcon.EnergyCareful;
                case Symbol.Scan: return (int) PwIcon.Scanner;
                default: return defaultIcon;
            }
        }
    }
}
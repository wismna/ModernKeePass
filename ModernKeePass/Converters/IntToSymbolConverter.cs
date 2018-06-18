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
                case PwIcon.Expired: return Symbol.Cancel;
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
            switch (symbol)
            {
                case Symbol.Delete:
                    return PwIcon.TrashBin;
                default:
                    return PwIcon.Folder;
            }
        }
    }
}
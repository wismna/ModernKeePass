using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Converters
{
    public class IconToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var icon = (Icon)value;
            switch (icon)
            {
                case Icon.Delete: return Symbol.Delete;
                case Icon.Edit: return Symbol.Edit;
                case Icon.Save: return Symbol.Save;
                case Icon.Cancel: return Symbol.Cancel;
                case Icon.Accept: return Symbol.Accept;
                case Icon.Home: return Symbol.Home;
                case Icon.Camera: return Symbol.Camera;
                case Icon.Setting: return Symbol.Setting;
                case Icon.Mail: return Symbol.Mail;
                case Icon.Find: return Symbol.Find;
                case Icon.Help: return Symbol.Help;
                case Icon.Clock: return Symbol.Clock;
                case Icon.Crop: return Symbol.Crop;
                case Icon.World: return Symbol.World;
                case Icon.Flag: return Symbol.Flag;
                case Icon.PreviewLink: return Symbol.PreviewLink;
                case Icon.Document: return Symbol.Document;
                case Icon.ProtectedDocument: return Symbol.ProtectedDocument;
                case Icon.ContactInfo: return Symbol.ContactInfo;
                case Icon.ViewAll: return Symbol.ViewAll;
                case Icon.Rotate: return Symbol.Rotate;
                case Icon.List: return Symbol.List;
                case Icon.Shop: return Symbol.Shop;
                case Icon.BrowsePhotos: return Symbol.BrowsePhotos;
                case Icon.Caption: return Symbol.Caption;
                case Icon.Repair: return Symbol.Repair;
                case Icon.Page: return Symbol.Page;
                case Icon.Paste: return Symbol.Paste;
                case Icon.Important: return Symbol.Important;
                case Icon.SlideShow: return Symbol.SlideShow;
                case Icon.MapDrive: return Symbol.MapDrive;
                case Icon.ContactPresence: return Symbol.ContactPresence;
                case Icon.Contact: return Symbol.Contact;
                case Icon.Folder: return Symbol.Folder;
                case Icon.View: return Symbol.View;
                case Icon.Permissions: return Symbol.Permissions;
                case Icon.Map: return Symbol.Map;
                case Icon.CellPhone: return Symbol.CellPhone;
                case Icon.OutlineStar: return Symbol.OutlineStar;
                case Icon.Calculator: return Symbol.Calculator;
                case Icon.Library: return Symbol.Library;
                case Icon.SyncFolder: return Symbol.SyncFolder;
                case Icon.GoToStart: return Symbol.GoToStart;
                case Icon.ZeroBars: return Symbol.ZeroBars;
                case Icon.FourBars: return Symbol.FourBars;
                case Icon.Scan: return Symbol.Scan;
                case Icon.ReportHacked: return Symbol.ReportHacked;
                case Icon.Stop: return Symbol.Stop;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var symbol = (Symbol)value;
            var defaultIcon = parameter != null ? int.Parse(parameter as string) : -1;
            switch (symbol)
            {
                case Symbol.Delete: return Icon.Delete;
                case Symbol.Edit: return Icon.Edit;
                case Symbol.Save: return Icon.Save;
                case Symbol.Cancel: return Icon.Cancel;
                case Symbol.Accept: return Icon.Accept;
                case Symbol.Home: return Icon.Home;
                case Symbol.Camera: return Icon.Camera;
                case Symbol.Setting: return Icon.Setting;
                case Symbol.Mail: return Icon.Mail;
                case Symbol.Find: return Icon.Find;
                case Symbol.Help: return Icon.Help;
                case Symbol.Clock: return Icon.Clock;
                case Symbol.Crop: return Icon.Crop;
                case Symbol.World: return Icon.World;
                case Symbol.Flag: return Icon.Flag;
                case Symbol.PreviewLink: return Icon.PreviewLink;
                case Symbol.Document: return Icon.Document;
                case Symbol.ProtectedDocument: return Icon.ProtectedDocument;
                case Symbol.ContactInfo: return Icon.ContactInfo;
                case Symbol.ViewAll: return Icon.ViewAll;
                case Symbol.Rotate: return Icon.Rotate;
                case Symbol.List: return Icon.List;
                case Symbol.Shop: return Icon.Shop;
                case Symbol.BrowsePhotos: return Icon.BrowsePhotos;
                case Symbol.Caption: return Icon.Caption;
                case Symbol.Repair: return Icon.Repair;
                case Symbol.Page: return Icon.Page;
                case Symbol.Paste: return Icon.Paste;
                case Symbol.Important: return Icon.Important;
                case Symbol.SlideShow: return Icon.SlideShow;
                case Symbol.MapDrive: return Icon.MapDrive;
                case Symbol.ContactPresence: return Icon.ContactPresence;
                case Symbol.Contact: return Icon.Contact;
                case Symbol.Folder: return Icon.Folder;
                case Symbol.View: return Icon.View;
                case Symbol.Permissions: return Icon.Permissions;
                case Symbol.Map: return Icon.Map;
                case Symbol.CellPhone: return Icon.CellPhone;
                case Symbol.OutlineStar: return Icon.OutlineStar;
                case Symbol.Calculator: return Icon.Calculator;
                case Symbol.Library: return Icon.Library;
                case Symbol.SyncFolder: return Icon.SyncFolder;
                case Symbol.GoToStart: return Icon.GoToStart;
                case Symbol.ZeroBars: return Icon.ZeroBars;
                case Symbol.FourBars: return Icon.FourBars;
                case Symbol.Scan: return Icon.Scan;
                case Symbol.ReportHacked: return Icon.ReportHacked;
                case Symbol.Stop: return Icon.Stop;
                default: return defaultIcon;
            }
        }
    }
}
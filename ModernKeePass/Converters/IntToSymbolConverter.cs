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
                //case PwIcon.PaperQ: return Symbol.;
                //case PwIcon.TerminalEncrypted: return Symbol.;
                case PwIcon.Console: return Symbol.SlideShow;
                case PwIcon.Printer: return Symbol.Scan;
                case PwIcon.ProgramIcons: return Symbol.GoToStart;
                //case PwIcon.Run: return Symbol.;
                case PwIcon.Settings:
                case PwIcon.Tool: return Symbol.Repair;
                case PwIcon.Archive: return Symbol.Crop;
                case PwIcon.Count: return Symbol.Calculator;
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
                /*case Symbol.Previous:
                    break;
                case Symbol.Next:
                    break;
                case Symbol.Play:
                    break;
                case Symbol.Pause:
                    break;
                case Symbol.Edit:
                    break;
                case Symbol.Save:
                    break;
                case Symbol.Clear:
                    break;*/
                case Symbol.Delete:
                    return PwIcon.TrashBin;
                /*case Symbol.Remove:
                    break;
                case Symbol.Add:
                    break;
                case Symbol.Cancel:
                    break;
                case Symbol.Accept:
                    break;
                case Symbol.More:
                    break;
                case Symbol.Redo:
                    break;
                case Symbol.Undo:
                    break;
                case Symbol.Home:
                    break;
                case Symbol.Up:
                    break;
                case Symbol.Forward:
                    break;
                case Symbol.Back:
                    break;
                case Symbol.Favorite:
                    break;
                case Symbol.Camera:
                    break;
                case Symbol.Setting:
                    break;
                case Symbol.Video:
                    break;
                case Symbol.Sync:
                    break;
                case Symbol.Download:
                    break;
                case Symbol.Mail:
                    break;
                case Symbol.Find:
                    break;
                case Symbol.Help:
                    break;
                case Symbol.Upload:
                    break;
                case Symbol.Emoji:
                    break;
                case Symbol.TwoPage:
                    break;
                case Symbol.LeaveChat:
                    break;
                case Symbol.MailForward:
                    break;
                case Symbol.Clock:
                    break;
                case Symbol.Send:
                    break;
                case Symbol.Crop:
                    break;
                case Symbol.RotateCamera:
                    break;
                case Symbol.People:
                    break;
                case Symbol.OpenPane:
                    break;
                case Symbol.ClosePane:
                    break;
                case Symbol.World:
                    break;
                case Symbol.Flag:
                    break;
                case Symbol.PreviewLink:
                    break;
                case Symbol.Globe:
                    break;
                case Symbol.Trim:
                    break;
                case Symbol.AttachCamera:
                    break;
                case Symbol.ZoomIn:
                    break;
                case Symbol.Bookmarks:
                    break;
                case Symbol.Document:
                    break;
                case Symbol.ProtectedDocument:
                    break;
                case Symbol.Page:
                    break;
                case Symbol.Bullets:
                    break;
                case Symbol.Comment:
                    break;
                case Symbol.MailFilled:
                    break;
                case Symbol.ContactInfo:
                    break;
                case Symbol.HangUp:
                    break;
                case Symbol.ViewAll:
                    break;
                case Symbol.MapPin:
                    break;
                case Symbol.Phone:
                    break;
                case Symbol.VideoChat:
                    break;
                case Symbol.Switch:
                    break;
                case Symbol.Contact:
                    break;
                case Symbol.Rename:
                    break;
                case Symbol.Pin:
                    break;
                case Symbol.MusicInfo:
                    break;
                case Symbol.Go:
                    break;
                case Symbol.Keyboard:
                    break;
                case Symbol.DockLeft:
                    break;
                case Symbol.DockRight:
                    break;
                case Symbol.DockBottom:
                    break;
                case Symbol.Remote:
                    break;
                case Symbol.Refresh:
                    break;
                case Symbol.Rotate:
                    break;
                case Symbol.Shuffle:
                    break;
                case Symbol.List:
                    break;
                case Symbol.Shop:
                    break;
                case Symbol.SelectAll:
                    break;
                case Symbol.Orientation:
                    break;
                case Symbol.Import:
                    break;
                case Symbol.ImportAll:
                    break;
                case Symbol.BrowsePhotos:
                    break;
                case Symbol.WebCam:
                    break;
                case Symbol.Pictures:
                    break;
                case Symbol.SaveLocal:
                    break;
                case Symbol.Caption:
                    break;
                case Symbol.Stop:
                    break;
                case Symbol.ShowResults:
                    break;
                case Symbol.Volume:
                    break;
                case Symbol.Repair:
                    break;
                case Symbol.Message:
                    break;
                case Symbol.Page2:
                    break;
                case Symbol.CalendarDay:
                    break;
                case Symbol.CalendarWeek:
                    break;
                case Symbol.Calendar:
                    break;
                case Symbol.Character:
                    break;
                case Symbol.MailReplyAll:
                    break;
                case Symbol.Read:
                    break;
                case Symbol.Link:
                    break;
                case Symbol.Account:
                    break;
                case Symbol.ShowBcc:
                    break;
                case Symbol.HideBcc:
                    break;
                case Symbol.Cut:
                    break;
                case Symbol.Attach:
                    break;
                case Symbol.Paste:
                    break;
                case Symbol.Filter:
                    break;
                case Symbol.Copy:
                    break;
                case Symbol.Emoji2:
                    break;
                case Symbol.Important:
                    break;
                case Symbol.MailReply:
                    break;
                case Symbol.SlideShow:
                    break;
                case Symbol.Sort:
                    break;
                case Symbol.Manage:
                    break;
                case Symbol.AllApps:
                    break;
                case Symbol.DisconnectDrive:
                    break;
                case Symbol.MapDrive:
                    break;
                case Symbol.NewWindow:
                    break;
                case Symbol.OpenWith:
                    break;
                case Symbol.ContactPresence:
                    break;
                case Symbol.Priority:
                    break;
                case Symbol.GoToToday:
                    break;
                case Symbol.Font:
                    break;
                case Symbol.FontColor:
                    break;
                case Symbol.Contact2:
                    break;
                case Symbol.Folder:
                    break;
                case Symbol.Audio:
                    break;
                case Symbol.Placeholder:
                    break;
                case Symbol.View:
                    break;
                case Symbol.SetLockScreen:
                    break;
                case Symbol.SetTile:
                    break;
                case Symbol.ClosedCaption:
                    break;
                case Symbol.StopSlideShow:
                    break;
                case Symbol.Permissions:
                    break;
                case Symbol.Highlight:
                    break;
                case Symbol.DisableUpdates:
                    break;
                case Symbol.UnFavorite:
                    break;
                case Symbol.UnPin:
                    break;
                case Symbol.OpenLocal:
                    break;
                case Symbol.Mute:
                    break;
                case Symbol.Italic:
                    break;
                case Symbol.Underline:
                    break;
                case Symbol.Bold:
                    break;
                case Symbol.MoveToFolder:
                    break;
                case Symbol.LikeDislike:
                    break;
                case Symbol.Dislike:
                    break;
                case Symbol.Like:
                    break;
                case Symbol.AlignRight:
                    break;
                case Symbol.AlignCenter:
                    break;
                case Symbol.AlignLeft:
                    break;
                case Symbol.Zoom:
                    break;
                case Symbol.ZoomOut:
                    break;
                case Symbol.OpenFile:
                    break;
                case Symbol.OtherUser:
                    break;
                case Symbol.Admin:
                    break;
                case Symbol.Street:
                    break;
                case Symbol.Map:
                    break;
                case Symbol.ClearSelection:
                    break;
                case Symbol.FontDecrease:
                    break;
                case Symbol.FontIncrease:
                    break;
                case Symbol.FontSize:
                    break;
                case Symbol.CellPhone:
                    break;
                case Symbol.ReShare:
                    break;
                case Symbol.Tag:
                    break;
                case Symbol.RepeatOne:
                    break;
                case Symbol.RepeatAll:
                    break;
                case Symbol.OutlineStar:
                    break;
                case Symbol.SolidStar:
                    break;
                case Symbol.Calculator:
                    break;
                case Symbol.Directions:
                    break;
                case Symbol.Target:
                    break;
                case Symbol.Library:
                    break;
                case Symbol.PhoneBook:
                    break;
                case Symbol.Memo:
                    break;
                case Symbol.Microphone:
                    break;
                case Symbol.PostUpdate:
                    break;
                case Symbol.BackToWindow:
                    break;
                case Symbol.FullScreen:
                    break;
                case Symbol.NewFolder:
                    break;
                case Symbol.CalendarReply:
                    break;
                case Symbol.UnSyncFolder:
                    break;
                case Symbol.ReportHacked:
                    break;
                case Symbol.SyncFolder:
                    break;
                case Symbol.BlockContact:
                    break;
                case Symbol.SwitchApps:
                    break;
                case Symbol.AddFriend:
                    break;
                case Symbol.TouchPointer:
                    break;
                case Symbol.GoToStart:
                    break;
                case Symbol.ZeroBars:
                    break;
                case Symbol.OneBar:
                    break;
                case Symbol.TwoBars:
                    break;
                case Symbol.ThreeBars:
                    break;
                case Symbol.FourBars:
                    break;
                case Symbol.Scan:
                    break;
                case Symbol.Preview:
                    break;*/
                default:
                    return PwIcon.Folder;
            }
        }
    }
}
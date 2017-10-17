using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.ViewModels;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace ModernKeePass.Pages
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage
    {
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper { get; }
        public GroupVm Model => (GroupVm)DataContext;

        public GroupDetailPage()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e) {}

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);

            if (!(e.Parameter is GroupVm)) return;
            DataContext = (GroupVm) e.Parameter;
            if (Model.IsEditMode)
                Task.Factory.StartNew(
                () => Dispatcher.RunAsync(CoreDispatcherPriority.Low,
                    () => TitleTextBox.Focus(FocusState.Programmatic)));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        #region Event Handlers

        private void groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GroupVm group;
            switch (LeftListView.SelectedIndex)
            {
                case -1:
                    return;
                case 0:
                    group = Model.CreateNewGroup();
                    break;
                default:
                    group = LeftListView.SelectedItem as GroupVm;
                    break;
            }
            Frame.Navigate(typeof(GroupDetailPage), group);
        }
        
        private void entries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EntryVm entry;
            switch (GridView.SelectedIndex)
            {
                case -1:
                    return;
                case 0:
                    entry = Model.CreateNewEntry();
                    break;
                default:
                    entry = GridView.SelectedItem as EntryVm;
                    break;
            }
            Frame.Navigate(typeof(EntryDetailPage), entry);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Are you sure you want to delete the whole group and all its entries?");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand("Delete", delete =>
            {
                ShowToast("Group", Model);
                Model.MarkForDelete();
                if (Frame.CanGoBack) Frame.GoBack();
            }));
            messageDialog.Commands.Add(new UICommand("Cancel"));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 1;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }
        
        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            // We need to synchronize the two lists (zoomed-in and zoomed-out) because the source is different
            if (e.IsSourceZoomedInView == false)
            {
                e.DestinationItem.Item = e.SourceItem.Item;
            }
        }

        private void SearchBox_OnSuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            var imageUri = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx://Assets/Logo.scale-80.png"));
            var results = Model.Entries.Skip(1).Where(e => e.Name.IndexOf(args.QueryText, StringComparison.OrdinalIgnoreCase) >= 0).Take(5);
            foreach (var result in results)
            {
                args.Request.SearchSuggestionCollection.AppendResultSuggestion(result.Name, result.ParentGroup.Name, result.Id, imageUri, string.Empty);
            }
        }

        private void SearchBox_OnResultSuggestionChosen(SearchBox sender, SearchBoxResultSuggestionChosenEventArgs args)
        {
            var entry = Model.Entries.Skip(1).FirstOrDefault(e => e.Id == args.Tag);
            Frame.Navigate(typeof(EntryDetailPage), entry);
        }

        #endregion

        private async void ShowToast(string entityType, IPwEntity entity)
        {
            // Construct the visuals of the toast
            var visual = new ToastVisual
            {
                BindingGeneric = new ToastBindingGeneric
                {
                    Children =
                    {
                        new AdaptiveText
                        {
                            Text = $"{entityType} {entity.Name} deleted."
                        }
                    }/*,

                    AppLogoOverride = new ToastGenericAppLogo()
                    {
                        Source = logo,
                        HintCrop = ToastGenericAppLogoCrop.Circle
                    }*/
                }
            };
            
            // Construct the actions for the toast (inputs and buttons)
            var actions = new ToastActionsCustom
            {
                Buttons =
                {
                    new ToastButton("Undo", new QueryString
                    {
                        { "action", "undo" },
                        { "entityType", entityType },
                        { "entityId", entity.Id }

                    }.ToString())
                }
            };

            // Now we can construct the final toast content
            var toastContent = new ToastContent
            {
                Visual = visual,
                Actions = actions,
                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "undo" },
                    { "entityType", "group" },
                    { "entityId", entity.Id }

                }.ToString()
            };

            // And create the toast notification
            var toastXml = new XmlDocument();
            toastXml.LoadXml(toastContent.GetContent());

            var visualXml = toastXml.GetElementsByTagName("visual")[0];
            ((XmlElement)visualXml.ChildNodes[0]).SetAttribute("template", "ToastText02");

            var toast = new ToastNotification(toastXml) {ExpirationTime = DateTime.Now.AddSeconds(5)};
            toast.Dismissed += Toast_Dismissed;

            /*var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var toastElements = notificationXml.GetElementsByTagName("text");
            toastElements[0].AppendChild(notificationXml.CreateTextNode($"{entityType} deleted"));
            toastElements[1].AppendChild(notificationXml.CreateTextNode("Click me to undo"));

            var toast = new ToastNotification(notificationXml)
            {
                ExpirationTime = DateTime.Now.AddSeconds(5)
            };*/
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void Toast_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            var app = (App)Application.Current;
            if (app.PendingDeleteQueue.Count == 0) return;
            var entity = app.PendingDeleteQueue.Dequeue();
            if (entity is GroupVm)
            {
                entity.CommitDelete();
            }
        }
    }
}

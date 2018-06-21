using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Actions
{
    public class RestoreEntityAction : DependencyObject, IAction
    {
        public IPwEntity Entity
        {
            get { return (IPwEntity)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("Entity", typeof(IPwEntity), typeof(RestoreEntityAction),
                new PropertyMetadata(null));

        public Frame Frame
        {
            get { return (Frame)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }

        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register("Frame", typeof(Frame), typeof(RestoreEntityAction),
                new PropertyMetadata(null));


        public object Execute(object sender, object parameter)
        {
            var resource = new ResourcesService();
            var type = Entity is GroupVm ? "Group" : "Entry";
            
            ToastNotificationHelper.ShowMovedToast(Entity, resource.GetResourceValue("EntityRestoredTitle"),
                resource.GetResourceValue($"{type}Restored"));
            if (Frame.CanGoBack) Frame.GoBack();

            return null;
        }
    }
}
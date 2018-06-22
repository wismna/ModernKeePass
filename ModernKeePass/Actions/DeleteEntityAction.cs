using System.Windows.Input;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Actions
{
    public class DeleteEntityAction : DependencyObject, IAction
    {
        public IPwEntity Entity
        {
            get { return (IPwEntity)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("Entity", typeof(IPwEntity), typeof(DeleteEntityAction),
                new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DeleteEntityAction),
                new PropertyMetadata(null));

        public object Execute(object sender, object parameter)
        {
            var resource = new ResourcesService();
            var type = Entity is GroupVm ? "Group" : "Entry";
            
            var message = Entity.IsRecycleOnDelete
                ? resource.GetResourceValue($"{type}RecyclingConfirmation")
                : resource.GetResourceValue($"{type}DeletingConfirmation");
            var text = Entity.IsRecycleOnDelete ? resource.GetResourceValue($"{type}Recycled") : resource.GetResourceValue($"{type}Deleted");
            MessageDialogHelper.ShowActionDialog(resource.GetResourceValue("EntityDeleteTitle"), message,
                resource.GetResourceValue("EntityDeleteActionButton"),
                resource.GetResourceValue("EntityDeleteCancelButton"), a =>
                {
                    ToastNotificationHelper.ShowMovedToast(Entity, resource.GetResourceValue("EntityDeleting"), text);
                    Entity.MarkForDelete(resource.GetResourceValue("RecycleBinTitle"));
                    Command.Execute(null);
                }, null).GetAwaiter();

            return null;
        }
    }
}
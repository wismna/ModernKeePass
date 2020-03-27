using System.Windows.Input;
using Windows.UI.Xaml;
using Microsoft.Xaml.Interactivity;
using ModernKeePass.Application.Resources.Queries;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Actions
{
    public class DeleteEntityAction : DependencyObject, IAction
    {
        public IVmEntity Entity
        {
            get { return (IVmEntity)GetValue(EntityProperty); }
            set { SetValue(EntityProperty, value); }
        }

        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("Entity", typeof(IVmEntity), typeof(DeleteEntityAction),
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
            var mediator = App.Mediator;
            var type = Entity is GroupVm ? "Group" : "Entry";
            
            var message = Entity.IsRecycleOnDelete
                ? mediator.Send(new GetResourceQuery { Key = $"{type}RecyclingConfirmation" })
                : mediator.Send(new GetResourceQuery { Key = $"{type}DeletingConfirmation" });
            var text = Entity.IsRecycleOnDelete ? 
                mediator.Send(new GetResourceQuery { Key = $"{type}Recycled" }) : 
                mediator.Send(new GetResourceQuery { Key = $"{type}Deleted" });
            MessageDialogHelper.ShowActionDialog(
                mediator.Send(new GetResourceQuery { Key = "EntityDeleteTitle" }).GetAwaiter().GetResult(), 
                message.GetAwaiter().GetResult(),
                mediator.Send(new GetResourceQuery { Key = "EntityDeleteActionButton" }).GetAwaiter().GetResult(),
                    mediator.Send(new GetResourceQuery { Key = "EntityDeleteCancelButton" }).GetAwaiter().GetResult(), async a =>
                {
                    ToastNotificationHelper.ShowMovedToast(Entity, await mediator.Send(new GetResourceQuery { Key = "EntityDeleting" }), await text);
                    await Entity.MarkForDelete(await mediator.Send(new GetResourceQuery { Key = "RecycleBinTitle"}));
                    Command.Execute(null);
                }, null).GetAwaiter();

            return null;
        }
    }
}
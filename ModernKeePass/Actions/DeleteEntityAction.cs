using System.Windows.Input;
using Windows.UI.Xaml;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Interactivity;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Actions
{
    public class DeleteEntityAction : DependencyObject, IAction
    {
        private readonly IMediator _mediator;

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

        public DeleteEntityAction() : this(App.Services.GetService<IMediator>()) { }

        public DeleteEntityAction(IMediator mediator)
        {
            _mediator = mediator;
        }

        public object Execute(object sender, object parameter)
        {
            var resource = new ResourceHelper();
            var type = Entity is GroupDetailVm ? "Group" : "Entry";
            var isRecycleOnDelete = _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult().IsRecycleBinEnabled;

            var message = isRecycleOnDelete
                ? resource.GetResourceValue($"{type}RecyclingConfirmation")
                : resource.GetResourceValue($"{type}DeletingConfirmation");
            var text = isRecycleOnDelete ? resource.GetResourceValue($"{type}Recycled") : resource.GetResourceValue($"{type}Deleted");
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
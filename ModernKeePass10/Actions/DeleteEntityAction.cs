using System.Windows.Input;
using Windows.UI.Xaml;
using Autofac;
using Microsoft.Xaml.Interactivity;
using ModernKeePass.Common;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Actions
{
    public class DeleteEntityAction : DependencyObject, IAction
    {
        private readonly IResourceService _resourceService;
        private readonly IDatabaseService _databaseService;

        public Entity Entity
        {
            get => (Entity)GetValue(EntityProperty);
            set => SetValue(EntityProperty, value);
        }

        public static readonly DependencyProperty EntityProperty =
            DependencyProperty.Register("Entity", typeof(Entity), typeof(DeleteEntityAction),
                new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DeleteEntityAction),
                new PropertyMetadata(null));

        public DeleteEntityAction(): this(App.Container.Resolve<IResourceService>(), App.Container.Resolve<IDatabaseService>())
        { }

        public DeleteEntityAction(IResourceService resourceService, IDatabaseService databaseService)
        {
            _resourceService = resourceService;
            _databaseService = databaseService;
        }

        public object Execute(object sender, object parameter)
        {
            var type = Entity is GroupEntity ? "Group" : "Entry";
            
            var message = _databaseService.IsRecycleBinEnabled
                ? _resourceService.GetResourceValue($"{type}RecyclingConfirmation")
                : _resourceService.GetResourceValue($"{type}DeletingConfirmation");
            var text = _databaseService.IsRecycleBinEnabled ? _resourceService.GetResourceValue($"{type}Recycled") : _resourceService.GetResourceValue($"{type}Deleted");
            MessageDialogHelper.ShowActionDialog(_resourceService.GetResourceValue("EntityDeleteTitle"), message,
                _resourceService.GetResourceValue("EntityDeleteActionButton"),
                _resourceService.GetResourceValue("EntityDeleteCancelButton"), a =>
                {
                    ToastNotificationHelper.ShowMovedToast(Entity, _resourceService.GetResourceValue("EntityDeleting"), text);
                    //Entity.MarkForDelete(_resourceService.GetResourceValue("RecycleBinTitle"));
                    Command.Execute(null);
                }, null).GetAwaiter();

            return null;
        }
    }
}
using Autofac;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class SettingsViewModel
    {
        private readonly IDatabaseService _database;

        public bool IsDatabaseOpened => _database != null && _database.IsOpen;

        public SettingsViewModel() : this(App.Container.Resolve<IDatabaseService>())
        { }

        public SettingsViewModel(IDatabaseService database)
        {
            _database = database;
        }
    }
}
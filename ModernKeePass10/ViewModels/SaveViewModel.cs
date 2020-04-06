using Autofac;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class SaveViewModel
    {
        private readonly IDatabaseService _database;
        public SaveViewModel() : this(App.Container.Resolve<IDatabaseService>()) { }

        public SaveViewModel(IDatabaseService database)
        {
            _database = database;
        }

        public void Save(bool close = true)
        {
            _database.Save();
            if (close) _database.Close();
        }

        public void Save(string token)
        {
            _database.SaveAs(new FileInfo
            {
                Path = token
            });
        }
    }
}
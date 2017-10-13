using Windows.Storage;
using Windows.UI.Xaml;

namespace ModernKeePass.ViewModels
{
    public class SaveVm
    {
        public void Save(bool close = true)
        {
            var app = (App)Application.Current;
            app.Database.Save();
            if (!close) return;
            app.Database.Close();
        }

        internal void Save(StorageFile file)
        {
            var app = (App)Application.Current;
            app.Database.Save(file);
        }
    }
}
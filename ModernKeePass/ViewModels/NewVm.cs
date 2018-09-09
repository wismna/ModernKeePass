using Windows.Storage;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Interfaces;
using ModernKeePassLib;

namespace ModernKeePass.ViewModels
{
    public class NewVm : OpenVm
    {
        public string Password { get; set; }

        public bool IsImportChecked { get; set; }

        public IStorageFile ImportFile { get; set; }

        public IFormat ImportFormat { get; set; }
        
        public void PopulateInitialData(IDatabaseService database, ISettingsService settings, IImportService<IFormat> importService)
        {
            if (settings.GetSetting<bool>("Sample") && !IsImportChecked) CreateSampleData(database);
            else if (IsImportChecked && ImportFile != null) importService.Import(ImportFormat, ImportFile, database);
        }

        private void CreateSampleData(IDatabaseService database)
        {
            var bankingGroup = database.RootGroup.AddNewGroup("Banking");
            bankingGroup.IconId = (int) Symbol.Calculator;

            var emailGroup = database.RootGroup.AddNewGroup("Email");
            emailGroup.IconId = (int) Symbol.Mail;

            var internetGroup = database.RootGroup.AddNewGroup("Internet");
            internetGroup.IconId = (int) Symbol.World;

            var sample1 = database.RootGroup.AddNewEntry();
            sample1.Name = "Sample Entry";
            sample1.UserName = "Username";
            sample1.Url = PwDefs.HomepageUrl;
            sample1.Password = "Password";
            sample1.Notes = "You may safely delete this sample";

            var sample2 = database.RootGroup.AddNewEntry();
            sample2.Name = "Sample Entry #2";
            sample2.UserName = "Michael321";
            sample2.Url = PwDefs.HelpUrl + "kb/testform.html";
            sample2.Password = "12345";
        }
    }
}

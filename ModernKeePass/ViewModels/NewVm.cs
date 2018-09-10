using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Converters;
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
            var converter = new IntToSymbolConverter();

            var bankingGroup = database.RootGroup.AddNewGroup("Banking");
            bankingGroup.IconId = (int)converter.ConvertBack(Symbol.Calculator, null, null, string.Empty);

            var emailGroup = database.RootGroup.AddNewGroup("Email");
            emailGroup.IconId = (int)converter.ConvertBack(Symbol.Mail, null, null, string.Empty);

            var internetGroup = database.RootGroup.AddNewGroup("Internet");
            internetGroup.IconId = (int)converter.ConvertBack(Symbol.World, null, null, string.Empty);

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

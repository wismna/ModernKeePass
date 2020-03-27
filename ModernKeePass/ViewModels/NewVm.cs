using Windows.Storage;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Converters;
using ModernKeePass.ImportFormats;
using ModernKeePass.Interfaces;
using ModernKeePassLib;

namespace ModernKeePass.ViewModels
{
    public class NewVm : OpenVm
    {
        private string _importFormatHelp;
        public string Password { get; set; }

        public bool IsImportChecked { get; set; }

        public IStorageFile ImportFile { get; set; }

        public string ImportFileExtensionFilter { get; set; } = "*";

        public IFormat ImportFormat { get; set; }

        public string ImportFormatHelp
        {
            get { return _importFormatHelp; }
            set
            {
                _importFormatHelp = value;
                OnPropertyChanged(nameof(ImportFormatHelp));
            }
        }

        public void PopulateInitialData(IDatabaseService database, ISettingsService settings, IImportService<IFormat> importService)
        {
            if (settings.GetSetting<bool>("Sample") && !IsImportChecked) CreateSampleData(database.RootGroup);
            else if (IsImportChecked && ImportFile != null && ! (ImportFormat is NullImportFormat)) importService.Import(ImportFormat, ImportFile, database.RootGroup);
        }

        private void CreateSampleData(GroupVm group)
        {
            var converter = new IntToSymbolConverter();

            var bankingGroup = group.AddNewGroup("Banking");
            bankingGroup.Icon = (int)converter.ConvertBack(Symbol.Calculator, null, null, string.Empty);

            var emailGroup = group.AddNewGroup("Email");
            emailGroup.Icon = (int)converter.ConvertBack(Symbol.Mail, null, null, string.Empty);

            var internetGroup = group.AddNewGroup("Internet");
            internetGroup.Icon = (int)converter.ConvertBack(Symbol.World, null, null, string.Empty);

            var sample1 = group.AddNewEntry();
            sample1.Title = "Sample Entry";
            sample1.UserName = "Username";
            sample1.Url = PwDefs.HomepageUrl;
            sample1.Password = "Password";
            sample1.Notes = "You may safely delete this sample";

            var sample2 = group.AddNewEntry();
            sample2.Title = "Sample Entry #2";
            sample2.UserName = "Michael321";
            sample2.Url = PwDefs.HelpUrl + "kb/testform.html";
            sample2.Password = "12345";
        }
    }
}

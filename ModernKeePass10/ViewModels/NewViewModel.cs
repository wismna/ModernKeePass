using System;
using Windows.Storage;
using Windows.Storage.AccessCache;
using ModernKeePass.Converters;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class NewViewModel : OpenViewModel
    {
        private string _importFormatHelp;
        private readonly IDatabaseService _databaseService;
        private readonly IImportService _importService;
        private readonly ISettingsService _settingsService;

        public string Password { get; set; }

        public bool IsImportChecked { get; set; }

        public IStorageFile ImportFile { get; set; }

        public string ImportFileExtensionFilter { get; set; } = "*";

        public ImportFormat ImportFormat { get; set; }

        public string ImportFormatHelp
        {
            get => _importFormatHelp;
            set
            {
                _importFormatHelp = value;
                OnPropertyChanged(nameof(ImportFormatHelp));
            }
        }

        public NewViewModel()
        { }

        public NewViewModel(IDatabaseService databaseService, IImportService importService, ISettingsService settingsService)
        {
            _databaseService = databaseService;
            _importService = importService;
            _settingsService = settingsService;
        }

        public void PopulateInitialData()
        {
            if (_settingsService.GetSetting<bool>("Sample") && !IsImportChecked) CreateSampleData(_databaseService.RootGroupEntity);
            else if (IsImportChecked && ImportFile != null)
            {
                var token = StorageApplicationPermissions.FutureAccessList.Add(ImportFile);
                _importService.Import(ImportFormat, token, _databaseService.RootGroupEntity);
                StorageApplicationPermissions.FutureAccessList.Remove(token);
            }
        }

        private void CreateSampleData(GroupEntity groupEntity)
        {
            var converter = new IconToSymbolConverter();

            groupEntity.SubGroups.Add(new GroupEntity
            {
                Name = "Banking",
                Icon = Icon.Calculator
            });

            groupEntity.SubGroups.Add(new GroupEntity
            {
                Name = "Email",
                Icon = Icon.Mail
            });

            groupEntity.SubGroups.Add(new GroupEntity
            {
                Name = "Internet",
                Icon = Icon.World
            });

            groupEntity.Entries.Add(new EntryEntity
            {
                Name = "Sample Entry",
                UserName = "Username",
                Url = new Uri("https://keepass.info"),
                Password = "Password",
                Notes = "You may safely delete this sample"
            });

            groupEntity.Entries.Add(new EntryEntity
            {
                Name = "Sample Entry #2",
                UserName = "Michael321",
                Url = new Uri("https://keepass.info/help/base/kb/testform.html"),
                Password = "12345"
            });
        }
    }
}

using Windows.Storage;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class NewVm : OpenVm
    {
        private string _importFormatHelp;

        public bool IsImportChecked { get; set; }

        public IStorageFile ImportFile { get; set; }

        public string ImportFileExtensionFilter { get; set; } = "*";

        public IImportFormat ImportFormat { get; set; }

        public string ImportFormatHelp
        {
            get { return _importFormatHelp; }
            set
            {
                _importFormatHelp = value;
                OnPropertyChanged(nameof(ImportFormatHelp));
            }
        }
    }
}

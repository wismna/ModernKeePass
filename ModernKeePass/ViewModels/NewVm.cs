using Windows.Storage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class NewVm : OpenVm
    {
        private readonly IMediator _mediator;
        private readonly ISettingsProxy _settings;
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

        public NewVm(): this(App.Services.GetService<IMediator>(), App.Services.GetService<ISettingsProxy>()) { }

        public NewVm(IMediator mediator, ISettingsProxy settings)
        {
            _mediator = mediator;
            _settings = settings;
        }
    }
}

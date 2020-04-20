using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.CreateDatabase;
using ModernKeePass.Common;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.ViewModels
{
    public class UpdateCredentialsViewModel : OpenDatabaseControlVm
    {
        private readonly ICredentialsProxy _credentials;
        private readonly ISettingsProxy _settings;
        private string _confirmPassword;
        

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }

        public double PasswordComplexityIndicator => _credentials.EstimatePasswordComplexity(Password);

        public new bool IsValid => HasPassword && Password == ConfirmPassword || HasKeyFile && KeyFilePath != string.Empty;

        public UpdateCredentialsViewModel(): this(App.Services.GetRequiredService<ICredentialsProxy>(), App.Services.GetRequiredService<ISettingsProxy>()) { }

        public UpdateCredentialsViewModel(ICredentialsProxy credentials, ISettingsProxy settings)
        {
            _credentials = credentials;
            _settings = settings;
        }

        public async Task CreateDatabase(FileInfo fileInfo)
        {
            await Mediator.Send(new CreateDatabaseCommand
            {
                FilePath = fileInfo.Path,
                KeyFilePath = HasKeyFile ? KeyFilePath : null,
                Password = HasPassword ? Password : null,
                Name = "New Database",
                CreateSampleData = _settings.GetSetting<bool>(Constants.Settings.Sample)
            });
        }
    }
}
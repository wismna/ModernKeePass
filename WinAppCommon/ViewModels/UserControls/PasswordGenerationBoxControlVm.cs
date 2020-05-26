using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Security.Commands.GeneratePassword;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels
{
    public class PasswordGenerationBoxControlVm: ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly ISettingsProxy _settings;
        private readonly ICredentialsProxy _credentials;
        private string _password;
        private bool _isRevealPassword;

        public double PasswordComplexityIndicator => _credentials.EstimatePasswordComplexity(Password);
        public double PasswordLength
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.PasswordLength, 25); }
            set
            {
                _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.PasswordLength, value);
                RaisePropertyChanged(() => PasswordLength);
            }
        }
        public bool UpperCasePatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.UpperCasePattern, true); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.UpperCasePattern, value); }
        }
        public bool LowerCasePatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.LowerCasePattern, true); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.LowerCasePattern, value); }
        }
        public bool DigitsPatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.DigitsPattern, true); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.DigitsPattern, value); }
        }
        public bool MinusPatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.MinusPattern, false); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.MinusPattern, value); }
        }
        public bool UnderscorePatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.UnderscorePattern, false); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.UnderscorePattern, value); }
        }
        public bool SpacePatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.SpacePattern, false); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.SpacePattern, value); }
        }
        public bool SpecialPatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.SpecialPattern, true); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.SpecialPattern, value); }
        }
        public bool BracketsPatternSelected
        {
            get { return _settings.GetSetting(Constants.Settings.PasswordGenerationOptions.BracketsPattern, false); }
            set { _settings.PutSetting(Constants.Settings.PasswordGenerationOptions.BracketsPattern, value); }
        }
        public string CustomChars { get; set; } = string.Empty;

        public string Password
        {
            get { return _password; }
            set
            {
                Set(() => Password, ref _password, value);
                RaisePropertyChanged(() => PasswordComplexityIndicator);
            }
        }

        public bool IsRevealPassword
        {
            get { return _isRevealPassword; }
            set { Set(() => IsRevealPassword, ref _isRevealPassword, value); }
        }

        public RelayCommand GeneratePasswordCommand { get; }

        public PasswordGenerationBoxControlVm(IMediator mediator, ISettingsProxy settings, ICredentialsProxy credentials)
        {
            _mediator = mediator;
            _settings = settings;
            _credentials = credentials;

            GeneratePasswordCommand = new RelayCommand(async () => await GeneratePassword());
        }

        private async Task GeneratePassword()
        {
            Password = await _mediator.Send(new GeneratePasswordCommand
            {
                BracketsPatternSelected = BracketsPatternSelected,
                CustomChars = CustomChars,
                DigitsPatternSelected = DigitsPatternSelected,
                LowerCasePatternSelected = LowerCasePatternSelected,
                MinusPatternSelected = MinusPatternSelected,
                PasswordLength = (int)PasswordLength,
                SpacePatternSelected = SpacePatternSelected,
                SpecialPatternSelected = SpecialPatternSelected,
                UnderscorePatternSelected = UnderscorePatternSelected,
                UpperCasePatternSelected = UpperCasePatternSelected
            });
            MessengerInstance.Send(new PasswordGeneratedMessage { Password = Password });
        }
    }
}
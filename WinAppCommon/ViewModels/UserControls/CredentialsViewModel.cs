﻿using System;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.CreateDatabase;
using ModernKeePass.Application.Database.Commands.UpdateCredentials;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Database.Queries.OpenDatabase;
using ModernKeePass.Application.Security.Commands.GenerateKeyFile;
using ModernKeePass.Application.Security.Queries.EstimatePasswordComplexity;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.ViewModels
{
    public class CompositeKeyVm : NotifyPropertyChangedBase
    {
        public enum StatusTypes
        {
            Normal = 0,
            Error = 1,
            Warning = 3,
            Success = 5
        }

        public bool HasPassword
        {
            get { return _hasPassword; }
            set
            {
                SetProperty(ref _hasPassword, value);
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public bool HasKeyFile
        {
            get { return _hasKeyFile; }
            set
            {
                SetProperty(ref _hasKeyFile, value);
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public bool IsValid => !_isOpening && (HasPassword || HasKeyFile && KeyFilePath != null);

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public int StatusType
        {
            get { return (int)_statusType; }
            set { SetProperty(ref _statusType, (StatusTypes)value); }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(PasswordComplexityIndicator));
                StatusType = (int)StatusTypes.Normal;
                Status = string.Empty;
            }
        }

        public string KeyFilePath
        {
            get { return _keyFilePath; }
            set
            {
                _keyFilePath = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }

        public string KeyFileText
        {
            get { return _keyFileText; }
            set { SetProperty(ref _keyFileText, value); }
        }

        public string RootGroupId { get; set; }

        public double PasswordComplexityIndicator => _mediator.Send(new EstimatePasswordComplexityQuery { Password = Password }).GetAwaiter().GetResult();

        private bool _hasPassword;
        private bool _hasKeyFile;
        private bool _isOpening;
        private string _password = string.Empty;
        private string _status;
        private StatusTypes _statusType;
        private string _keyFilePath;
        private string _keyFileText;
        private readonly IMediator _mediator;
        private readonly ISettingsProxy _settings;
        private readonly IResourceProxy _resource;

        public CompositeKeyVm() : this(
            App.Services.GetRequiredService<IMediator>(),
            App.Services.GetRequiredService<ISettingsProxy>(),
            App.Services.GetRequiredService<IResourceProxy>())
        { }

        public CompositeKeyVm(IMediator mediator, ISettingsProxy settings, IResourceProxy resource)
        {
            _mediator = mediator;
            _settings = settings;
            _resource = resource;
            _keyFileText = _resource.GetResourceValue("CompositeKeyDefaultKeyFile");
        }

        public async Task<bool> OpenDatabase(string databaseFilePath, bool createNew)
        {
            try
            {
                _isOpening = true;
                OnPropertyChanged(nameof(IsValid));
                if (createNew)
                {
                    await _mediator.Send(new CreateDatabaseCommand
                    {
                        FilePath = databaseFilePath,
                        KeyFilePath = HasKeyFile ? KeyFilePath : null,
                        Password = HasPassword ? Password : null,
                        Name = "New Database",
                        CreateSampleData = _settings.GetSetting<bool>("Sample")
                    });
                }
                else
                {
                    await _mediator.Send(new OpenDatabaseQuery
                    {
                        FilePath = databaseFilePath,
                        KeyFilePath = HasKeyFile ? KeyFilePath : null,
                        Password = HasPassword ? Password : null,
                    });
                }
                RootGroupId = (await _mediator.Send(new GetDatabaseQuery())).RootGroupId;
                return true;
            }
            catch (ArgumentException)
            {
                var errorMessage = new StringBuilder($"{_resource.GetResourceValue("CompositeKeyErrorOpen")}\n");
                if (HasPassword) errorMessage.AppendLine(_resource.GetResourceValue("CompositeKeyErrorUserPassword"));
                if (HasKeyFile) errorMessage.AppendLine(_resource.GetResourceValue("CompositeKeyErrorUserKeyFile"));
                UpdateStatus(errorMessage.ToString(), StatusTypes.Error);
            }
            catch (Exception e)
            {
                var error = $"{_resource.GetResourceValue("CompositeKeyErrorOpen")}{e.Message}";
                UpdateStatus(error, StatusTypes.Error);
            }
            finally
            {
                _isOpening = false;
                OnPropertyChanged(nameof(IsValid));
            }
            return false;
        }

        private void UpdateStatus(string text, StatusTypes type)
        {
            Status = text;
            StatusType = (int)type;
        }
    }
}
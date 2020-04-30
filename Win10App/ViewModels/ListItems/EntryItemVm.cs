using System;
using System.Collections.Generic;
using Windows.UI;
using GalaSoft.MvvmLight;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels.ListItems
{
    public class EntryItemVm : ObservableObject
    {
        public EntryEntity EntryEntity { get; }
        public GroupItemVm Parent { get; }

        public bool HasExpired => HasExpirationDate && EntryEntity.ExpirationDate < DateTime.Now;

        public bool HasUrl => !string.IsNullOrEmpty(Url);

        public double PasswordComplexityIndicator => _securityService.EstimatePasswordComplexity(Password);

        public string Name
        {
            get => EntryEntity.Name;
            set
            {
                EntryEntity.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string UserName
        {
            get => EntryEntity.UserName;
            set => EntryEntity.UserName = value;
        }

        public string Password
        {
            get => EntryEntity.Password;
            set
            {
                EntryEntity.Password = value;
                OnPropertyChanged();
            }
        }

        public string Url
        {
            get => EntryEntity.Url?.ToString();
            set => EntryEntity.Url = new Uri(value);
        }

        public string Notes
        {
            get => EntryEntity.Notes;
            set => EntryEntity.Notes = value;
        }

        public Icon Icon
        {
            get => HasExpired ? Icon.Important : EntryEntity.Icon;
            set => EntryEntity.Icon = value;
        }

        public DateTimeOffset ExpiryDate
        {
            get => EntryEntity.ExpirationDate;
            set
            {
                if (!HasExpirationDate) return;
                EntryEntity.ExpirationDate = value;
            }
        }

        public TimeSpan ExpiryTime
        {
            get => EntryEntity.ExpirationDate.TimeOfDay;
            set
            {
                if (!HasExpirationDate) return;
                EntryEntity.ExpirationDate = EntryEntity.ExpirationDate.Date.Add(value);
            }
        }

        public bool HasExpirationDate
        {
            get => EntryEntity.HasExpirationDate;
            set
            {
                EntryEntity.HasExpirationDate = value;
                OnPropertyChanged();
            }
        }

        public Color BackgroundColor
        {
            get => Color.FromArgb(EntryEntity.BackgroundColor.A, EntryEntity.BackgroundColor.R, EntryEntity.BackgroundColor.G, EntryEntity.BackgroundColor.B);
            set
            {
                EntryEntity.BackgroundColor = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
                OnPropertyChanged();
            }
        }

        public Color ForegroundColor
        {
            get => Color.FromArgb(EntryEntity.ForegroundColor.A, EntryEntity.ForegroundColor.R, EntryEntity.ForegroundColor.G, EntryEntity.ForegroundColor.B);
            set
            {
                EntryEntity.ForegroundColor = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
                OnPropertyChanged();
            }
        }

        public IEnumerable<EntryItemVm> History
        {
            get
            {
                var history = new Stack<EntryItemVm>();
                foreach (var historyEntry in EntryEntity.History)
                {
                    history.Push(new EntryItemVm(_securityService, historyEntry, Parent));
                }
                history.Push(this);

                return history;
            }
        }
        public Dictionary<string, string> AdditionalFields => EntryEntity.AdditionalFields;

        public EntryItemVm(EntryEntity entryEntity, GroupItemVm parentGroup): this(App.Container.Resolve<ISecurityService>(), entryEntity, parentGroup)
        { }

        public EntryItemVm(ISecurityService securityService, EntryEntity entryEntity, GroupItemVm parentGroup)
        {
            _securityService = securityService;
            EntryEntity = entryEntity;
            Parent = parentGroup;
        }

        public override string ToString() => EntryEntity.LastModificationDate.ToString("g");
    }
}
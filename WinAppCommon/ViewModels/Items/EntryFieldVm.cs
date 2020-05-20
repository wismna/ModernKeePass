using System.Linq;
using GalaSoft.MvvmLight;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.ViewModels.ListItems
{
    public class EntryFieldVm: ViewModelBase
    {
        private readonly ICryptographyClient _cryptography;
        private string _name;
        private string _value;
        private bool _isProtected;

        public string Name
        {
            get { return _name; }
            set
            {
                var newName = EntryFieldName.StandardFieldNames.Contains(value) ? $"{value}_1" : value;
                MessengerInstance.Send(new EntryFieldNameChangedMessage { OldName = Name, NewName = newName, Value = Value, IsProtected = IsProtected});
                Set(nameof(Name), ref _name, newName);
            }
        }

        public string DisplayValue => IsProtected? "*****" : _value;

        public string Value
        {
            get
            {
                return IsProtected? _cryptography.UnProtect(_value).GetAwaiter().GetResult() : _value;
            }
            set
            {
                var protectedValue = IsProtected ? _cryptography.Protect(value).GetAwaiter().GetResult() : value;
                MessengerInstance.Send(new EntryFieldValueChangedMessage { FieldName = Name, FieldValue = protectedValue, IsProtected = IsProtected });
                Set(nameof(Value), ref _value, protectedValue);
                RaisePropertyChanged(nameof(DisplayValue));
            }
        }

        public bool IsProtected
        {
            get { return _isProtected; }
            set
            {
                Set(nameof(IsProtected), ref _isProtected, value);
                if (!string.IsNullOrEmpty(Name)) Value = value ? _value : _cryptography.UnProtect(_value).GetAwaiter().GetResult();
            }
        }
        
        public EntryFieldVm(ICryptographyClient cryptography)
        {
            _cryptography = cryptography;
        }

        public void Initialize(string fieldName, string fieldValue, bool isProtected)
        {
            _name = fieldName;
            _value = fieldValue;
            _isProtected = isProtected;
        }
    }
}
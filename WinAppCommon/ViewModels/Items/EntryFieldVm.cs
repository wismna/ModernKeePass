using System.Linq;
using GalaSoft.MvvmLight;
using Messages;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.ViewModels.ListItems
{
    public class EntryFieldVm: ViewModelBase
    {
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

        public string Value
        {
            get { return _value; }
            set
            {
                MessengerInstance.Send(new EntryFieldValueChangedMessage { FieldName = Name, FieldValue = value, IsProtected = IsProtected });
                Set(nameof(Value), ref _value, value);
            }
        }
        public bool IsProtected
        {
            get { return _isProtected; }
            set
            {
                MessengerInstance.Send(new EntryFieldValueChangedMessage { FieldName = Name, FieldValue = Value, IsProtected = value });
                Set(nameof(IsProtected), ref _isProtected, value);
            }
        }
        
        public EntryFieldVm(string fieldName, string fieldValue, bool isProtected)
        {
            _name = fieldName;
            _value = fieldValue;
            _isProtected = isProtected;
        }
    }
}
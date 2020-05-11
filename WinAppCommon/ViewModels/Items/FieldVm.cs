using GalaSoft.MvvmLight;
using Messages;

namespace ModernKeePass.ViewModels.ListItems
{
    public class FieldVm: ViewModelBase
    {
        private string _name;
        private string _value;

        public string Name
        {
            get { return _name; }
            set
            {
                MessengerInstance.Send(new EntryFieldNameChangedMessage { OldName = Name, NewName = value, Value = Value });
                Set(nameof(Name), ref _name, value);
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                MessengerInstance.Send(new EntryFieldValueChangedMessage { FieldName = Name, FieldValue = value });
                Set(nameof(Value), ref _value, value);
            }
        }

        public FieldVm(string fieldName, string fieldValue)
        {
            _name = fieldName;
            _value = fieldValue;
        }
    }
}
﻿namespace Messages
{
    public class EntryFieldValueChangedMessage
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public bool IsProtected { get; set; }
    }
}
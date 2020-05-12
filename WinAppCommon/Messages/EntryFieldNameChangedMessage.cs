namespace Messages
{
    public class EntryFieldNameChangedMessage
    {
        public string OldName { get; set; }
        public string NewName { get; set; }
        public string Value { get; set; }
        public bool IsProtected { get; set; }
    }
}
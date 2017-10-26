namespace ModernKeePass.Interfaces
{
    public interface IHasSelectableObject
    {
        ISelectableModel SelectedItem { get; set; }
    }
}

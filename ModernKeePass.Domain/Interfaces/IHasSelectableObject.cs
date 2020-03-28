namespace ModernKeePass.Domain.Interfaces
{
    public interface IHasSelectableObject
    {
        ISelectableModel SelectedItem { get; set; }
    }
}

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IHasSelectableObject
    {
        ISelectableModel SelectedItem { get; set; }
    }
}

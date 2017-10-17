using Windows.UI.Xaml.Controls;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Interfaces
{
    public interface IPwEntity
    {
        GroupVm ParentGroup { get; }
        Symbol IconSymbol { get; }
        string Id { get; }
        string Name { get; set; }
        bool IsEditMode { get; }

        void CommitDelete();
    }
}
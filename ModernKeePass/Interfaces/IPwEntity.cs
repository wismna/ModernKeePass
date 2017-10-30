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

        /// <summary>
        /// Delete from Model
        /// </summary>
        void CommitDelete();
        /// <summary>
        /// Restore ViewModel
        /// </summary>
        void UndoDelete();
        /// <summary>
        /// Save changes to Model
        /// </summary>
        void Save();
        /// <summary>
        /// Delete from ViewModel
        /// </summary>
        void MarkForDelete();
    }
}
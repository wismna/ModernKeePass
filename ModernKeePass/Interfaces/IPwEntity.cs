using System.Collections.Generic;
using System.Windows.Input;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Interfaces
{
    public interface IPwEntity
    {
        GroupVm ParentGroup { get; }
        GroupVm PreviousGroup { get; }
        int IconId { get; }
        string Id { get; }
        string Name { get; set; }
        IEnumerable<IPwEntity> BreadCrumb { get; }
        bool IsEditMode { get; }
        bool IsRecycleOnDelete { get; }

        /// <summary>
        /// Save changes to Model
        /// </summary>
        ICommand SaveCommand { get; }
        /// <summary>
        /// Restore ViewModel
        /// </summary>
        ICommand UndoDeleteCommand { get; }
        /// <summary>
        /// Move a entity to the destination group
        /// </summary>
        /// <param name="destination">The destination to move the entity to</param>
        void Move(GroupVm destination);
        /// <summary>
        /// Delete from Model
        /// </summary>
        void CommitDelete();
        /// <summary>
        /// Delete from ViewModel
        /// </summary>
        void MarkForDelete(string recycleBinTitle);
    }
}
using System.Collections.Generic;
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
        /// Move a entity to the destination group
        /// </summary>
        /// <param name="destination">The destination to move the entity to</param>
        void Move(GroupVm destination);
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
        void MarkForDelete(string recycleBinTitle);
    }
}
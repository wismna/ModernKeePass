using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Interfaces
{
    public interface IVmEntity
    {
        GroupVm ParentGroup { get; }
        GroupVm PreviousGroup { get; }
        int Icon { get; }
        string Id { get; }
        string Title { get; set; }
        IEnumerable<IVmEntity> BreadCrumb { get; }
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
        Task CommitDelete();
        /// <summary>
        /// Delete from ViewModel
        /// </summary>
        Task MarkForDelete(string recycleBinTitle);
    }
}
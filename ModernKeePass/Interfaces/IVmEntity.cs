using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Application.Group.Models;

namespace ModernKeePass.Interfaces
{
    public interface IVmEntity
    {
        Symbol Icon { get; }
        string Id { get; }
        string Title { get; set; }
        IEnumerable<GroupVm> BreadCrumb { get; }
        bool IsEditMode { get; }

        /// <summary>
        /// Save changes to Model
        /// </summary>
        ICommand SaveCommand { get; }
        /// <summary>
        /// Move a entity to the destination group
        /// </summary>
        /// <param name="destination">The destination to move the entity to</param>
        Task Move(GroupVm destination);
        /// <summary>
        /// Delete from ViewModel
        /// </summary>
        Task MarkForDelete(string recycleBinTitle);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// Move a entity to the destination group
        /// </summary>
        /// <param name="destination">The destination to move the entity to</param>
        Task Move(GroupVm destination);
    }
}
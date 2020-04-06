using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: NotifyPropertyChangedBase
    {
        private readonly IRecentProxy _recent;
        public bool IsFileSelected => !string.IsNullOrEmpty(Path);

        public string Name { get; private set; }
        public string Path { get; private set; }

        public OpenVm(): this(App.Services.GetService<IRecentProxy>()) { }

        public OpenVm(IRecentProxy recent)
        {
            _recent = recent;
        }
        
        public async Task OpenFile(FileInfo file)
        {
            Name = file.Name;
            Path = file.Path;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(IsFileSelected));
            await AddToRecentList(file);
        }
        
        private async Task AddToRecentList(FileInfo file)
        {
            await _recent.Add(file);
        }
    }
}

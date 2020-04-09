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
        private string _name;
        private string _path;
        private string _token;
        public bool IsFileSelected => !string.IsNullOrEmpty(Path);

        public string Token
        {
            get { return _token; }
            set { SetProperty(ref _token, value); }
        }

        public string Name
        {
            get { return _name; }
            private set { SetProperty(ref _name, value); }
        }

        public string Path  
        {
            get { return _path; }
            private set { SetProperty(ref _path, value); }
        }

        public OpenVm(): this(App.Services.GetRequiredService<IRecentProxy>()) { }

        public OpenVm(IRecentProxy recent)
        {
            _recent = recent;
        }
        
        public async Task OpenFile(FileInfo file)
        {
            Token = file.Id;
            Name = file.Name;
            Path = file.Path;
            OnPropertyChanged(nameof(IsFileSelected));
            await AddToRecentList(file);
        }
        
        private async Task AddToRecentList(FileInfo file)
        {
            await _recent.Add(file);
        }
    }
}

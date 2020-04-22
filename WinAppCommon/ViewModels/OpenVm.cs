using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: ObservableObject
    {
        private readonly IRecentProxy _recent;
        private string _name;
        private string _path;
        private string _token;
        public bool IsFileSelected => !string.IsNullOrEmpty(Path);

        public string Token
        {
            get { return _token; }
            set { Set(() => Token, ref _token, value); }
        }

        public string Name
        {
            get { return _name; }
            private set { Set(() => Name, ref _name, value); }
        }

        public string Path  
        {
            get { return _path; }
            private set { Set(() => Path, ref _path, value); }
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
            RaisePropertyChanged(nameof(IsFileSelected));
            await AddToRecentList(file);
        }
        
        private async Task AddToRecentList(FileInfo file)
        {
            await _recent.Add(file);
        }
    }
}

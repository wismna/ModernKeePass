using System.Threading.Tasks;
using Windows.Storage;
using ModernKeePass.Interfaces;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Services
{
    public class ImportService : IImportService<IFormat>
    {
        public async Task Import(IFormat format, IStorageFile source, GroupVm group)
        {
            var data = await format.Import(source);

            foreach (var entity in data)
            {
                var entry = group.AddNewEntry();
                entry.Title = entity["0"];
                entry.UserName = entity["1"];
                entry.Password = entity["2"];
                if (entity.Count > 3) entry.Url = entity["3"];
                if (entity.Count > 4) entry.Notes = entity["4"];
            }
        }
    }
}
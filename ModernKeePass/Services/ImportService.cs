using ModernKeePass.Interfaces;
using Windows.Storage;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Services
{
    public class ImportService : IImportService<IFormat>
    {
        public void Import(IFormat format, IStorageFile source, IDatabaseService databaseService)
        {
            var entities = (GroupVm)format.Import(source);

            foreach (var entry in entities.Entries)
            {
                databaseService.RootGroup.Entries.Add(entry);
            }

            foreach (var subGroup in entities.Groups)
            {
                databaseService.RootGroup.Groups.Add(subGroup);
            }
        }
    }
}
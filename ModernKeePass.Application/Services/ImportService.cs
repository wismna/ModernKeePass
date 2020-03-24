using System;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Application.Services
{
    public class ImportService: IImportService
    {
        private readonly Func<ImportFormat, IImportFormat> _importFormatProviders;

        public ImportService(Func<ImportFormat, IImportFormat> importFormatProviders)
        {
            _importFormatProviders = importFormatProviders;
        }

        public async Task Import(ImportFormat format, string filePath, Group group)
        {
            var importProvider = _importFormatProviders(format);
            var data = await importProvider.Import(filePath);

            /*foreach (var entity in data)
            {
                var entry = group.AddNewEntry();
                entry.Name = entity["0"];
                entry.UserName = entity["1"];
                entry.Password = entity["2"];
                if (entity.Count > 3) entry.Url = entity["3"];
                if (entity.Count > 4) entry.Notes = entity["4"];
            }*/
        }
    }
}

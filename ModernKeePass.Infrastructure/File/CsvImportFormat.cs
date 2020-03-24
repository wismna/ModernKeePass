using System.Collections.Generic;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Infrastructure.File
{
    public class CsvImportFormat: IImportFormat
    {
        private readonly IFileProxy _fileService;
        private const bool HasHeaderRow = true;
        private const char Delimiter = ';';
        private const char LineDelimiter = '\n';

        public CsvImportFormat(IFileProxy fileService)
        {
            _fileService = fileService;
        }

        public async Task<List<Dictionary<string, string>>> Import(string path)
        {
            var parsedResult = new List<Dictionary<string, string>>();
            var content = await _fileService.OpenTextFile(path);
            foreach (var line in content)
            {
                var fields = line.Split(Delimiter);
                var recordItem = new Dictionary<string, string>();
                var i = 0;
                foreach (var field in fields)
                {
                    recordItem.Add(i.ToString(), field);
                    i++;
                }
                parsedResult.Add(recordItem);
            }

            return parsedResult;
        }
    }
}
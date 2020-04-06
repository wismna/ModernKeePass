using System.Collections.Generic;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Infrastructure.File
{
    public class CsvImportFormat: IImportFormat
    {
        private const bool HasHeaderRow = true;
        private const char Delimiter = ';';
        private const char LineDelimiter = '\n';
        
        public async Task<List<Dictionary<string, string>>> Import(IList<string> fileContents)
        {
            var parsedResult = new List<Dictionary<string, string>>();
            foreach (var line in fileContents)
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
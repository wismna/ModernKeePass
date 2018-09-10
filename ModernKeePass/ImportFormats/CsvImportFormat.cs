using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ImportFormats
{
    public class CsvImportFormat: IFormat
    {
        public bool HasHeaderRow { get; set; } = true;
        public char Delimiter { get; set; } = ';';
        public char LineDelimiter { get; set; } = '\n';

        public async Task<List<Dictionary<string, string>>> Import(IStorageFile source)
        {
            var parsedResult = new List<Dictionary<string, string>>();
            var content = await FileIO.ReadLinesAsync(source);
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
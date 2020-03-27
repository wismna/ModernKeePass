using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using MediatR;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Entry.Commands.SetFieldValue;
using ModernKeePass.Application.Group.Commands.CreateEntry;
using ModernKeePass.Application.Group.Commands.CreateGroup;
using ModernKeePass.Converters;
using ModernKeePass.Domain.Enums;
using ModernKeePass.ImportFormats;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class NewVm : OpenVm
    {
        private readonly IMediator _mediator;
        private string _importFormatHelp;
        public string Password { get; set; }

        public bool IsImportChecked { get; set; }

        public IStorageFile ImportFile { get; set; }

        public string ImportFileExtensionFilter { get; set; } = "*";

        public IFormat ImportFormat { get; set; }

        public string ImportFormatHelp
        {
            get { return _importFormatHelp; }
            set
            {
                _importFormatHelp = value;
                OnPropertyChanged(nameof(ImportFormatHelp));
            }
        }

        public NewVm(): this(App.Mediator) { }

        public NewVm(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PopulateInitialData(ISettingsService settings, IImportService<IFormat> importService)
        {
            var database = await _mediator.Send(new GetDatabaseQuery());
            if (settings.GetSetting<bool>("Sample") && !IsImportChecked) await CreateSampleData(database.RootGroup);
            else if (IsImportChecked && ImportFile != null && ! (ImportFormat is NullImportFormat)) importService.Import(ImportFormat, ImportFile, database.RootGroup);
        }

        private async Task CreateSampleData(Application.Group.Models.GroupVm group)
        {
            /*var converter = new IntToSymbolConverter();

            var bankingGroup = group.AddNewGroup("Banking");
            bankingGroup.Icon = (int)converter.ConvertBack(Symbol.Calculator, null, null, string.Empty);

            var emailGroup = group.AddNewGroup("Email");
            emailGroup.Icon = (int)converter.ConvertBack(Symbol.Mail, null, null, string.Empty);

            var internetGroup = group.AddNewGroup("Internet");
            internetGroup.Icon = (int)converter.ConvertBack(Symbol.World, null, null, string.Empty);

            var sample1 = group.AddNewEntry();
            sample1.Title = "Sample Entry";
            sample1.UserName = "Username";
            sample1.Url = PwDefs.HomepageUrl;
            sample1.Password = "Password";
            sample1.Notes = "You may safely delete this sample";

            var sample2 = group.AddNewEntry();
            sample2.Title = "Sample Entry #2";
            sample2.UserName = "Michael321";
            sample2.Url = PwDefs.HelpUrl + "kb/testform.html";
            sample2.Password = "12345";*/

            var bankingGroup = await _mediator.Send(new CreateGroupCommand {ParentGroup = group, Name = "Banking"});
            var emailGroup = await _mediator.Send(new CreateGroupCommand {ParentGroup = group, Name = "Email" });
            var internetGroup = await _mediator.Send(new CreateGroupCommand {ParentGroup = group, Name = "Internet" });

            var sample1 = await _mediator.Send(new CreateEntryCommand { ParentGroup = group });
            await _mediator.Send(new SetFieldValueCommand {EntryId = sample1.Id, FieldName = EntryFieldName.Title, FieldValue = "Sample Entry"});
            await _mediator.Send(new SetFieldValueCommand {EntryId = sample1.Id, FieldName = EntryFieldName.UserName, FieldValue = "Username" });
            await _mediator.Send(new SetFieldValueCommand {EntryId = sample1.Id, FieldName = EntryFieldName.Password, FieldValue = "Password" });
            await _mediator.Send(new SetFieldValueCommand {EntryId = sample1.Id, FieldName = EntryFieldName.Url, FieldValue = "https://keepass.info/" });
            await _mediator.Send(new SetFieldValueCommand {EntryId = sample1.Id, FieldName = EntryFieldName.Notes, FieldValue = "You may safely delete this sample" });

            var sample2 = await _mediator.Send(new CreateEntryCommand { ParentGroup = group });
            await _mediator.Send(new SetFieldValueCommand {EntryId = sample2.Id, FieldName = EntryFieldName.Title, FieldValue = "Sample Entry #2"});
            await _mediator.Send(new SetFieldValueCommand {EntryId = sample2.Id, FieldName = EntryFieldName.UserName, FieldValue = "Michael321" });
            await _mediator.Send(new SetFieldValueCommand {EntryId = sample2.Id, FieldName = EntryFieldName.Password, FieldValue = "12345" });
            await _mediator.Send(new SetFieldValueCommand {EntryId = sample2.Id, FieldName = EntryFieldName.Url, FieldValue = "https://keepass.info/help/kb/testform.html" });
        }
    }
}

using System.Threading.Tasks;
using Windows.Storage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Entry.Commands.SetFieldValue;
using ModernKeePass.Application.Group.Commands.CreateEntry;
using ModernKeePass.Application.Group.Commands.CreateGroup;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Application.Group.Queries.GetGroup;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.ViewModels
{
    public class NewVm : OpenVm
    {
        private readonly IMediator _mediator;
        private readonly ISettingsProxy _settings;
        private string _importFormatHelp;
        public string Password { get; set; }

        public bool IsImportChecked { get; set; }

        public IStorageFile ImportFile { get; set; }

        public string ImportFileExtensionFilter { get; set; } = "*";

        public IImportFormat ImportFormat { get; set; }

        public string ImportFormatHelp
        {
            get { return _importFormatHelp; }
            set
            {
                _importFormatHelp = value;
                OnPropertyChanged(nameof(ImportFormatHelp));
            }
        }

        public NewVm(): this(App.Services.GetService<IMediator>(), App.Services.GetService<ISettingsProxy>()) { }

        public NewVm(IMediator mediator, ISettingsProxy settings)
        {
            _mediator = mediator;
            _settings = settings;
        }

        public async Task<GroupVm> PopulateInitialData()
        {
            var database = await _mediator.Send(new GetDatabaseQuery());
            var rootGroup = await _mediator.Send(new GetGroupQuery {Id = database.RootGroupId});
            if (_settings.GetSetting<bool>("Sample") && !IsImportChecked) await CreateSampleData(rootGroup);
            return rootGroup;
        }

        private async Task CreateSampleData(GroupVm group)
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

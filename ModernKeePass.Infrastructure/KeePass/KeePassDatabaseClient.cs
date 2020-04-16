using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;
using ModernKeePassLib;
using ModernKeePassLib.Collections;
using ModernKeePassLib.Cryptography.KeyDerivation;
using ModernKeePassLib.Interfaces;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Security;
using ModernKeePassLib.Serialization;
using ModernKeePassLib.Utility;

namespace ModernKeePass.Infrastructure.KeePass
{
    public class KeePassDatabaseClient: IDatabaseProxy, IDisposable
    {
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;
        private readonly PwDatabase _pwDatabase = new PwDatabase();
        private Credentials _credentials;
        // Flag: Has Dispose already been called?
        private bool _disposed;
        
        // Main information
        public bool IsOpen => (_pwDatabase?.IsOpen).GetValueOrDefault();
        public string Name => _pwDatabase?.Name;
        public string RootGroupId => _pwDatabase?.RootGroup.Uuid.ToHexString();

        // TODO: find a correct place for this
        public string FileAccessToken { get; set; }
        public int Size { get; set; }
        public bool IsDirty { get; set; }

        // Settings
        public bool IsRecycleBinEnabled
        {
            get { return _pwDatabase.RecycleBinEnabled; }
            set { _pwDatabase.RecycleBinEnabled = value; }
        }

        public string RecycleBinId 
        {
            get
            {
                return _pwDatabase.RecycleBinUuid.ToHexString();
            }
            set
            {
                _pwDatabase.RecycleBinUuid = BuildIdFromString(value);
                _pwDatabase.RecycleBinChanged = _dateTime.Now;
            }
        }

        public string CipherId
        {
            get { return _pwDatabase.DataCipherUuid.ToHexString(); }
            set { _pwDatabase.DataCipherUuid = BuildIdFromString(value); }
        }

        public string KeyDerivationId
        {
            get { return _pwDatabase.KdfParameters.KdfUuid.ToHexString(); }
            set
            {
                _pwDatabase.KdfParameters = KdfPool.Engines
                    .FirstOrDefault(e => e.Uuid.Equals(BuildIdFromString(value)))?.GetDefaultParameters();
            }
        }

        public string Compression
        {
            get { return _pwDatabase.Compression.ToString("G"); }
            set { _pwDatabase.Compression = (PwCompressionAlgorithm) Enum.Parse(typeof(PwCompressionAlgorithm), value); }
        }

        public KeePassDatabaseClient(IMapper mapper, IDateTime dateTime)
        {
            _mapper = mapper;
            _dateTime = dateTime;
        }

        public async Task Open(byte[] file, Credentials credentials)
        {
            try
            {
                await Task.Run(() => 
                { 
                    var compositeKey = CreateCompositeKey(credentials);
                    var ioConnection = IOConnectionInfo.FromByteArray(file);

                    _pwDatabase.Open(ioConnection, compositeKey, new NullStatusLogger());

                    _credentials = credentials;
                });
            }
            catch (InvalidCompositeKeyException ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public async Task ReOpen(byte[] file)
        { 
            await Open(file, _credentials);
        }

        public async Task Create(Credentials credentials, string name, DatabaseVersion version = DatabaseVersion.V2)
        {
            try
            {
                await Task.Run(() =>
                {
                    var compositeKey = CreateCompositeKey(credentials);
                    var ioConnection = IOConnectionInfo.FromByteArray(new byte[] {});

                    _pwDatabase.New(ioConnection, compositeKey);
                    _pwDatabase.Name = name;
                    _pwDatabase.RootGroup.Name = name;

                    _credentials = credentials;

                    switch (version)
                    {
                        case DatabaseVersion.V4:
                            _pwDatabase.KdfParameters = KdfPool.Get("Argon2").GetDefaultParameters();
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public async Task<byte[]> SaveDatabase()
        {
            return await Task.Run(() =>
            {
                _pwDatabase.Save(new NullStatusLogger());
                return _pwDatabase.IOConnectionInfo.Bytes;
            });
        }

        public async Task<byte[]> SaveDatabase(byte[] newFileContents)
        {
            return await Task.Run(() =>
            {
                _pwDatabase.SaveAs(IOConnectionInfo.FromByteArray(newFileContents), true, new NullStatusLogger());
                return _pwDatabase.IOConnectionInfo.Bytes;
            });
        }
        
        public void CloseDatabase()
        {
            _pwDatabase?.Close();
        }

        public async Task AddEntry(string parentGroupId, string entryId)
        {
            await Task.Run(() =>
            {
                var parentPwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(parentGroupId), true);
                var pwEntry = _pwDatabase.RootGroup.FindEntry(BuildIdFromString(entryId), true);
                parentPwGroup.AddEntry(pwEntry, true);
            });
        }

        public async Task MoveEntry(string parentGroupId, string entryId, int index)
        {
            await Task.Run(() =>
            {
                var parentPwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(parentGroupId), true);
                var pwEntry = _pwDatabase.RootGroup.FindEntry(BuildIdFromString(entryId), true);
                var currentIndex = (uint)parentPwGroup.Entries.IndexOf(pwEntry);

                parentPwGroup.Entries.RemoveAt(currentIndex);
                parentPwGroup.Entries.Insert((uint)index, pwEntry);
            });
        }

        public async Task AddGroup(string parentGroupId, string groupId)
        {
            await Task.Run(() =>
            {
                var parentPwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(parentGroupId), true);
                var pwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(groupId), true);
                parentPwGroup.AddGroup(pwGroup, true);
            });
        }
        public async Task RemoveEntry(string parentGroupId, string entryId)
        {
            await Task.Run(() =>
            {
                var parentPwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(parentGroupId), true);
                var pwEntry = parentPwGroup.FindEntry(BuildIdFromString(entryId), false);
                parentPwGroup.Entries.Remove(pwEntry);
            });
        }

        public async Task RemoveGroup(string parentGroupId, string groupId)
        {
            await Task.Run(() =>
            {
                var parentPwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(parentGroupId), true);
                var pwGroup = parentPwGroup.FindGroup(BuildIdFromString(groupId), false);
                parentPwGroup.Groups.Remove(pwGroup);
            });
        }

        public void DeleteEntity(string entityId)
        {
            _pwDatabase.DeletedObjects.Add(new PwDeletedObject(BuildIdFromString(entityId), _dateTime.Now));
        }

        public void UpdateEntry(string entryId, string fieldName, object fieldValue)
        {
            var pwEntry = _pwDatabase.RootGroup.FindEntry(BuildIdFromString(entryId), true);

            switch (fieldName)
            {
                case EntryFieldName.Title:
                case EntryFieldName.UserName:
                case EntryFieldName.Password:
                case EntryFieldName.Notes:
                case EntryFieldName.Url:
                    pwEntry.Strings.Set(EntryFieldMapper.MapFieldToPwDef(fieldName), new ProtectedString(true, fieldValue.ToString()));
                    break;
                case EntryFieldName.HasExpirationDate:
                    pwEntry.Expires = (bool)fieldValue;
                    break;
                case EntryFieldName.ExpirationDate:
                    pwEntry.ExpiryTime = (DateTime)fieldValue;
                    break;
                case EntryFieldName.Icon:
                    pwEntry.IconId = IconMapper.MapIconToPwIcon((Icon)fieldValue);
                    break;
                case EntryFieldName.BackgroundColor:
                    pwEntry.BackgroundColor = (Color)fieldValue;
                    break;
                case EntryFieldName.ForegroundColor:
                    pwEntry.ForegroundColor = (Color)fieldValue;
                    break;
            }
        }

        public void AddHistory(string entryId)
        {
            var pwEntry = _pwDatabase.RootGroup.FindEntry(BuildIdFromString(entryId), true);
            pwEntry.Touch(true);
            pwEntry.CreateBackup(null);
        }

        public void RestoreFromHistory(string entryId, int historyIndex)
        {
            var pwEntry = _pwDatabase.RootGroup.FindEntry(BuildIdFromString(entryId), true);
            pwEntry.RestoreFromBackup((uint)historyIndex, _pwDatabase);
            pwEntry.Touch(true);
        }

        public void DeleteHistory(string entryId, int historyIndex)
        {
            var pwEntry = _pwDatabase.RootGroup.FindEntry(BuildIdFromString(entryId), true);
            pwEntry.History.RemoveAt((uint)historyIndex);
        }

        public void UpdateGroup(string groupId)
        {
            throw new NotImplementedException();
        }
        
        public EntryEntity CreateEntry(string parentGroupId)
        {
            var pwEntry = new PwEntry(true, true);
            var parentPwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(parentGroupId), true);
            parentPwGroup.AddEntry(pwEntry, true);

            return _mapper.Map<EntryEntity>(pwEntry);
        }

        public GroupEntity CreateGroup(string parentGroupId, string name, bool isRecycleBin = false)
        {
            var pwGroup = new PwGroup(true, true, name, isRecycleBin? PwIcon.TrashBin : PwIcon.Folder);
            var parentPwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(parentGroupId), true);
            parentPwGroup.AddGroup(pwGroup, true);
            if (isRecycleBin) _pwDatabase.RecycleBinUuid = pwGroup.Uuid;

            return _mapper.Map<GroupEntity>(pwGroup);
        }

        public void SortEntries(string groupId)
        {
            var pwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(groupId), true);
            var comparer = new PwEntryComparer(PwDefs.TitleField, true, false);
            pwGroup.Entries.Sort(comparer);
        }

        public void SortSubGroups(string groupId)
        {
            var pwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(groupId), true);
            pwGroup.SortSubGroups(false);
        }

        public EntryEntity GetEntry(string id)
        {
            var pwEntry = _pwDatabase.RootGroup.FindEntry(BuildIdFromString(id), true);
            return _mapper.Map<EntryEntity>(pwEntry);
        }

        public GroupEntity GetGroup(string id)
        {
            var pwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(id), true);
            return _mapper.Map<GroupEntity>(pwGroup);
        }

        public void UpdateCredentials(Credentials credentials)
        {
            _pwDatabase.MasterKey = CreateCompositeKey(credentials);
        }

        public IEnumerable<EntryEntity> Search(string groupId, string text)
        {
            var pwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(groupId), true);
            var searchResults = new PwObjectList<PwEntry>();
            pwGroup.SearchEntries(new SearchParameters
            {
                ComparisonMode = StringComparison.OrdinalIgnoreCase,
                SearchInTitles = true,
                //SearchInUserNames = true,
                SearchString = text

            }, searchResults);
            return searchResults.Select(e => _mapper.Map<EntryEntity>(e));
        }

        private CompositeKey CreateCompositeKey(Credentials credentials)
        {
            var compositeKey = new CompositeKey();
            if (!string.IsNullOrEmpty(credentials.Password)) compositeKey.AddUserKey(new KcpPassword(credentials.Password));
            if (credentials.KeyFileContents != null)
            {
                compositeKey.AddUserKey(new KcpKeyFile(IOConnectionInfo.FromByteArray(credentials.KeyFileContents)));
            }
            return compositeKey;
        }
        
        private PwUuid BuildIdFromString(string id)
        {
            return new PwUuid(MemUtil.HexStringToByteArray(id));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _pwDatabase?.Close();
            }

            _disposed = true;
        }
    }
}
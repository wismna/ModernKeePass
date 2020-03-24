using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Exceptions;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.Cipher;
using ModernKeePassLib.Cryptography.KeyDerivation;
using ModernKeePassLib.Interfaces;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Serialization;
using ModernKeePassLib.Utility;

namespace ModernKeePass.Infrastructure.KeePass
{
    public class KeePassDatabaseClient: IDatabaseProxy
    {
        private readonly ISettingsProxy _settings;
        private readonly IFileProxy _fileService;
        private readonly IMapper _mapper;
        private readonly PwDatabase _pwDatabase = new PwDatabase();
        private string _fileAccessToken;
        private CompositeKey _compositeKey;

        public bool IsOpen => (_pwDatabase?.IsOpen).GetValueOrDefault();

        public GroupEntity RecycleBin { get; set; }

        public BaseEntity Cipher
        {
            get
            {
                var cipher = CipherPool.GlobalPool.GetCipher(_pwDatabase.DataCipherUuid);
                return new BaseEntity
                {
                    Id = cipher.CipherUuid.ToHexString(),
                    Name = cipher.DisplayName
                };
            }
            set => _pwDatabase.DataCipherUuid = BuildIdFromString(value.Id);
        }

        public BaseEntity KeyDerivation
        {
            get
            {
                var keyDerivation = KdfPool.Engines.First(e => e.Uuid.Equals(_pwDatabase.KdfParameters.KdfUuid));
                return new BaseEntity
                {
                    Id = keyDerivation.Uuid.ToHexString(),
                    Name = keyDerivation.Name
                };
            } 
            set => _pwDatabase.KdfParameters = KdfPool.Engines
                .FirstOrDefault(e => e.Uuid.Equals(BuildIdFromString(value.Name)))?.GetDefaultParameters();
        }

        public string Compression
        {
            get => _pwDatabase.Compression.ToString("G");
            set => _pwDatabase.Compression = (PwCompressionAlgorithm)Enum.Parse(typeof(PwCompressionAlgorithm), value);
        }

        public KeePassDatabaseClient(ISettingsProxy settings, IFileProxy fileService, IMapper mapper)
        {
            _settings = settings;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<DatabaseEntity> Open(FileInfo fileInfo, Credentials credentials)
        {
            try
            {
                _compositeKey = await CreateCompositeKey(credentials);
                var ioConnection = await BuildConnectionInfo(fileInfo);

                _pwDatabase.Open(ioConnection, _compositeKey, new NullStatusLogger());

                _fileAccessToken = fileInfo.Path;

                return new DatabaseEntity
                {
                    RootGroupEntity = BuildHierarchy(_pwDatabase.RootGroup)
                };
            }
            catch (InvalidCompositeKeyException ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public async Task<DatabaseEntity> Create(FileInfo fileInfo, Credentials credentials)
        {
            _compositeKey = await CreateCompositeKey(credentials);
            var ioConnection = await BuildConnectionInfo(fileInfo);

            _pwDatabase.New(ioConnection, _compositeKey);

            var fileFormat = _settings.GetSetting<string>("DefaultFileFormat");
            switch (fileFormat)
            {
                case "4":
                    _pwDatabase.KdfParameters = KdfPool.Get("Argon2").GetDefaultParameters();
                    break;
            }

            _fileAccessToken = fileInfo.Path;

            // TODO: create sample data depending on settings
            return new DatabaseEntity
            {
                RootGroupEntity = BuildHierarchy(_pwDatabase.RootGroup)
            };
        }

        public async Task SaveDatabase()
        {
            if (!_pwDatabase.IsOpen) return;
            try
            {
                _pwDatabase.Save(new NullStatusLogger());
                await _fileService.WriteBinaryContentsToFile(_fileAccessToken, _pwDatabase.IOConnectionInfo.Bytes);
            }
            catch (Exception e)
            {
                throw new SaveException(e);
            }
        }

        public async Task SaveDatabase(FileInfo fileInfo)
        {
            try
            {
                var newFileContents = await _fileService.OpenBinaryFile(fileInfo.Path);
                _pwDatabase.SaveAs(IOConnectionInfo.FromByteArray(newFileContents), true, new NullStatusLogger());
                await _fileService.WriteBinaryContentsToFile(fileInfo.Path, _pwDatabase.IOConnectionInfo.Bytes);

                _fileService.ReleaseFile(_fileAccessToken);
                _fileAccessToken = fileInfo.Path;
            }
            catch (Exception e)
            {
                throw new SaveException(e);
            }
        }
        
        public void CloseDatabase()
        {
            _pwDatabase?.Close();
            _fileService.ReleaseFile(_fileAccessToken);
        }

        public async Task AddEntry(GroupEntity parentGroupEntity, EntryEntity entry)
        {
            await Task.Run(() =>
            {
                var parentPwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(parentGroupEntity.Id), true);

                var pwEntry = new PwEntry(true, true);
                _mapper.Map(entry, pwEntry);
                parentPwGroup.AddEntry(pwEntry, true);
                entry.Id = pwEntry.Uuid.ToHexString();
            });
        }
        public async Task AddGroup(GroupEntity parentGroupEntity, GroupEntity group)
        {
            await Task.Run(() =>
            {
                var parentPwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(parentGroupEntity.Id), true);

                var pwGroup = new PwGroup(true, true)
                {
                    Name = group.Name
                };
                parentPwGroup.AddGroup(pwGroup, true);
                group.Id = pwGroup.Uuid.ToHexString();

            });
        }

        public Task UpdateEntry(EntryEntity entry)
        {
            throw new NotImplementedException();
        }

        public Task UpdateGroup(GroupEntity group)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteEntry(EntryEntity entry)
        {
            await Task.Run(() =>
            {
                var pwEntry = _pwDatabase.RootGroup.FindEntry(BuildIdFromString(entry.Id), true);
                var id = pwEntry.Uuid;
                pwEntry.ParentGroup.Entries.Remove(pwEntry);

                if (_pwDatabase.RecycleBinEnabled)
                {
                    _pwDatabase.DeletedObjects.Add(new PwDeletedObject(id, DateTime.UtcNow));
                }
            });
        }
        public async Task DeleteGroup(GroupEntity group)
        {
            await Task.Run(() =>
            {
                var pwGroup = _pwDatabase.RootGroup.FindGroup(BuildIdFromString(group.Id), true);
                var id = pwGroup.Uuid;
                pwGroup.ParentGroup.Groups.Remove(pwGroup);

                if (_pwDatabase.RecycleBinEnabled)
                {
                    _pwDatabase.DeletedObjects.Add(new PwDeletedObject(id, DateTime.UtcNow));
                }
            });
        }

        public async Task UpdateCredentials(Credentials credentials)
        {
            _pwDatabase.MasterKey = await CreateCompositeKey(credentials);
        }

        private async Task<CompositeKey> CreateCompositeKey(Credentials credentials)
        {
            var compositeKey = new CompositeKey();
            if (!string.IsNullOrEmpty(credentials.Password)) compositeKey.AddUserKey(new KcpPassword(credentials.Password));
            if (!string.IsNullOrEmpty(credentials.KeyFilePath))
            {
                var kcpFileContents = await _fileService.OpenBinaryFile(credentials.KeyFilePath);
                compositeKey.AddUserKey(new KcpKeyFile(IOConnectionInfo.FromByteArray(kcpFileContents)));
            }
            return compositeKey;
        }

        private async Task<IOConnectionInfo> BuildConnectionInfo(FileInfo fileInfo)
        {
            var fileContents = await _fileService.OpenBinaryFile(fileInfo.Path);
            return IOConnectionInfo.FromByteArray(fileContents);
        }

        private GroupEntity BuildHierarchy(PwGroup pwGroup)
        {
            // TODO: build entity hierarchy in an iterative way or implement lazy loading
            var group = new GroupEntity
            {
                Id = pwGroup.Uuid.ToHexString(),
                Name = pwGroup.Name,
                Icon = IconMapper.MapPwIconToIcon(pwGroup.IconId),
                Entries = pwGroup.Entries.Select(e => _mapper.Map<EntryEntity>(e)).ToList(),
                SubGroups = pwGroup.Groups.Select(BuildHierarchy).ToList()
            };
            return group;
        }

        private PwUuid BuildIdFromString(string id)
        {
            return new PwUuid(MemUtil.HexStringToByteArray(id));
        }
    }
}
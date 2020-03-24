using System;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Application.Services
{
    public class DatabaseService: IDatabaseService
    {
        private readonly IDatabaseProxy _databaseProxy;

        public string Name { get; private set; }
        public bool IsOpen { get; private set; }
        public Domain.Entities.GroupEntity RootGroupEntity { get; private set; }
        public Domain.Entities.GroupEntity RecycleBin
        {
            get => _databaseProxy.RecycleBin;
            set => _databaseProxy.RecycleBin = value;
        }
        public Entity Cipher
        {
            get => _databaseProxy.Cipher;
            set => _databaseProxy.Cipher = value;
        }
        public Entity KeyDerivation
        {
            get => _databaseProxy.KeyDerivation;
            set => _databaseProxy.KeyDerivation = value;
        }
        public string Compression
        {
            get => _databaseProxy.Compression;
            set => _databaseProxy.Compression = value;
        }
        public bool IsRecycleBinEnabled => RecycleBin != null;

        public DatabaseService(IDatabaseProxy databaseProxy)
        {
            _databaseProxy = databaseProxy;
        }

        public async Task Open(FileInfo fileInfo, Credentials credentials)
        {
            RootGroupEntity = await _databaseProxy.Open(fileInfo, credentials);
            Name = RootGroupEntity?.Name;
            IsOpen = true;
        }

        public async Task Create(FileInfo fileInfo, Credentials credentials)
        {
            RootGroupEntity = await _databaseProxy.Create(fileInfo, credentials);
            Name = RootGroupEntity?.Name;
            IsOpen = true;
        }

        public async Task Save()
        {
            await _databaseProxy.SaveDatabase();
        }

        public async Task SaveAs(FileInfo fileInfo)
        {
            await _databaseProxy.SaveDatabase(fileInfo);
        }

        public Task CreateRecycleBin(Domain.Entities.GroupEntity recycleBinGroupEntity)
        {
            throw new NotImplementedException();
        }
        
        public async Task UpdateCredentials(Credentials credentials)
        {
            await _databaseProxy.UpdateCredentials(credentials);
            await Save();
        }

        public void Close()
        {
            _databaseProxy.CloseDatabase();
            IsOpen = false;
        }

        public async Task AddEntity(GroupEntity parentEntity, Entity entity)
        {
            await _databaseProxy.AddEntity(parentEntity, entity);
            //await Save();
        }

        public async Task UpdateEntity(Entity entity)
        {
            await _databaseProxy.UpdateEntity(entity);
        }

        public async Task DeleteEntity(Entity entity)
        {
            if (IsRecycleBinEnabled) await AddEntity(RecycleBin, entity);
            await _databaseProxy.DeleteEntity(entity);
            //await Save();
        }
    }
}
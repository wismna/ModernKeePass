﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Mappings;
using ModernKeePassLib;

namespace ModernKeePass.ViewModels
{
    public class GroupVm : NotifyPropertyChangedBase, IPwEntity, ISelectableModel
    {
        public GroupVm ParentGroup { get; private set; }
        public GroupVm PreviousGroup { get; private set; }
        public ObservableCollection<EntryVm> Entries { get; set; } = new ObservableCollection<EntryVm>();
        
        public ObservableCollection<GroupVm> Groups { get; set; } = new ObservableCollection<GroupVm>();

        public int EntryCount => Entries.Count() - 1;
        public FontWeight FontWeight => _pwGroup == null ? FontWeights.Bold : FontWeights.Normal;
        public int GroupCount => Groups.Count - 1;
        public PwUuid IdUuid => _pwGroup?.Uuid;
        public string Id => IdUuid?.ToHexString();
        public bool IsNotRoot => ParentGroup != null;

        public bool ShowRestore => IsNotRoot && ParentGroup.IsSelected;
        /// <summary>
        /// Is the Group the database Recycle Bin?
        /// </summary>
        public bool IsSelected
        {
            get { return _database.RecycleBinEnabled && _database.RecycleBin?.Id == Id; }
            set
            {
                if (value && _pwGroup != null) _database.RecycleBin = this;
            }
        }

        public IOrderedEnumerable<IGrouping<char, EntryVm>> EntriesZoomedOut => from e in Entries
            where !e.IsFirstItem
            group e by e.Name.FirstOrDefault() into grp
            orderby grp.Key
            select grp;

        public string Name
        {
            get { return _pwGroup == null ? "< New group >" : _pwGroup.Name; }
            set { _pwGroup.Name = value; }
        }

        public Symbol IconSymbol
        {
            get
            {
                if (_pwGroup == null) return Symbol.Add;
                var result = PwIconToSegoeMapping.GetSymbolFromIcon(_pwGroup.IconId);
                return result == Symbol.More ? Symbol.Folder : result;
            }
            set { _pwGroup.IconId = PwIconToSegoeMapping.GetIconFromSymbol(value); }
        }
        
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { SetProperty(ref _isEditMode, value); }
        }

        public string Path
        {
            get
            {
                if (ParentGroup == null) return string.Empty;
                var path = new StringBuilder(ParentGroup.Path);
                path.Append($" > {ParentGroup.Name}");
                return path.ToString();
            }
        }

        private readonly PwGroup _pwGroup;
        private readonly IDatabase _database;
        private bool _isEditMode;

        public GroupVm() {}

        internal GroupVm(PwGroup pwGroup, GroupVm parent, PwUuid recycleBinId = null) : this(pwGroup, parent,
            (Application.Current as App)?.Database, recycleBinId)
        { }

        public GroupVm(PwGroup pwGroup, GroupVm parent, IDatabase database, PwUuid recycleBinId = null)
        {
            _pwGroup = pwGroup;
            _database = database;
            ParentGroup = parent;

            if (recycleBinId != null && _pwGroup.Uuid.Equals(recycleBinId)) _database.RecycleBin = this;
            Entries = new ObservableCollection<EntryVm>(pwGroup.Entries.Select(e => new EntryVm(e, this)).OrderBy(e => e.Name));
            Entries.Insert(0, new EntryVm ());
            Groups = new ObservableCollection<GroupVm>(pwGroup.Groups.Select(g => new GroupVm(g, this, recycleBinId)).OrderBy(g => g.Name));
            Groups.Insert(0, new GroupVm ());
        }

        public GroupVm AddNewGroup(string name = "")
        {
            var pwGroup = new PwGroup(true, true, name, PwIcon.Folder);
            _pwGroup.AddGroup(pwGroup, true);
            var newGroup = new GroupVm(pwGroup, this) {Name = name, IsEditMode = string.IsNullOrEmpty(name)};
            Groups.Add(newGroup);
            return newGroup;
        }
        
        public EntryVm AddNewEntry()
        {
            var pwEntry = new PwEntry(true, true);
            _pwGroup.AddEntry(pwEntry, true);
            var newEntry = new EntryVm(pwEntry, this) {IsEditMode = true};
            Entries.Add(newEntry);
            return newEntry;
        }

        public void AddPwEntry(PwEntry entry)
        {
            _pwGroup.AddEntry(entry, true);
        }

        public void RemovePwEntry(PwEntry entry)
        {
            _pwGroup.Entries.Remove(entry);
        }

        public void MarkForDelete()
        {
            if (_database.RecycleBinEnabled && _database.RecycleBin?.IdUuid == null)
                _database.CreateRecycleBin();
            Move(_database.RecycleBinEnabled && !IsSelected ? _database.RecycleBin : null);
        }


        public void UndoDelete()
        {
            Move(PreviousGroup);
        }

        public void Move(GroupVm destination)
        {
            PreviousGroup = ParentGroup;
            PreviousGroup.Groups.Remove(this);
            PreviousGroup._pwGroup.Groups.Remove(_pwGroup);
            if (destination == null)
            {
                _database.AddDeletedItem(IdUuid);
                return;
            }
            ParentGroup = destination;
            ParentGroup.Groups.Add(this);
            ParentGroup._pwGroup.AddGroup(_pwGroup, true);
        }

        public void CommitDelete()
        {
            _pwGroup.ParentGroup.Groups.Remove(_pwGroup);
            if (_database.RecycleBinEnabled && !PreviousGroup.IsSelected) _database.RecycleBin._pwGroup.AddGroup(_pwGroup, true);
            else _database.AddDeletedItem(IdUuid);
        }

        public void Save()
        {
            _database.Save();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

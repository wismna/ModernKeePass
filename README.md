[<img src="https://geogeob.visualstudio.com/_apis/public/build/definitions/04291454-0e79-47a4-9502-5bd374804ccf/2/badge"/>](https://geogeob.visualstudio.com/_apis/public/build/index?definitionId=2)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ModernKeePass&metric=alert_status)](https://sonarcloud.io/dashboard?id=ModernKeePass)

# Introduction
**ModernKeePass** is port of the classic Windows application KeePass 2.x for the Windows Store.  
It does not aim to be feature perfect, but aims at being simple to use and user-friendly.

You can get it [here](https://www.microsoft.com/en-us/store/p/modernkeepass/9mwq48zk8nhv?rtc=1)

# Features
- Works on Windows 10, 8.1 and RT
- Full compatibility with classic KeePass databases: read and write support of KDBX files version 2, 3 and 4
- Open database with password and key file
- Create new databases
- Create new key files
- Create, edit and delete groups and entries
- Generate passwords for entries
- Use Recycle Bin
- Search entries
- Sort and reorder entries
- View, delete and restore from entry history
- Use Semantic Zoom to see your entries in a grouped mode
- List recently opened databases
- Open a database from Windows Explorer
- Change database encryption
- Change database compression
- Change database key derivation
- Displays and change entry colors and icons
- Move entries and groups from a group to another
- Entry custom fields (view, add, update, delete)
- Entry attachments (view, add, delete)

# Build and Test
1. Clone the repository
2. Build the main app (the library reference dll is actually a NuGet dependency, built from the [**ModernKeePassLib** project](https://github.com/wismna/ModernKeePassLib))
3. Edit the `.appxmanifest` file to select another certificate (you can create one using Visual Studio or *certutil.exe*)

# Contribute
I'm not the best at creating nice assets, so if anyone would like to contribute some nice icons, it would be awesome :)
Otherwise, there are still many things left to implement:
- Multi entry selection (for delete, or move)
- Import existing data from CSV, JSON, or XML
- Open database from URL (and maybe some clouds?)

# Credits
*Dominik Reichl* for the [KeePass application](https://keepass.info/), library and file format  
*David Lechner* for his [PCL adapatation](https://github.com/dlech/KeePass2PCL) of the KeePass Library and the related tests which served as an inspiration basis for my own adaptation

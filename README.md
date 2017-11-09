# Introduction
**ModernKeePass** is port of the classic Windows application KeePass 2.x for the Windows Store.  
It does not aim to be feature perfect, but aims at being simple to use and user-friendly.

You can get it [here](https://www.microsoft.com/fr-fr/store/p/modernkeepass/9mwq48zk8nhv?rtc=1)

# Features
- Read and write support of KDBX files version 2, 3 and 4
- Open database with password and key file
- Create new databases
- Create, edit and delete groups
- Create, edit and delete entries
- Generate passwords for entries
- Use Recycle Bin
- Search entries
- Use Semantic Zoom to see your entries in a grouped mode
- List recently opened databases
- Open database from Windows Explorer
- Change database encryption
- Change database compression
- Change database key derivation
- Displays entry colors and icons (set in KeePass)

# Build and Test
1. Clone the repository
2. Build the main app (the library reference dll is actually a NuGet dependency, built from the [**ModernKeePassLib** project](../ModernKeePassLib/README.md))

# Contribute
I'm not the best at creating nice assets, so if anyone would like to contribute some nice icons, it would be awesome :)
Otherwise, there are still many things left to implement:
- Entry custom fields
- Multi entry selection (for delete, or move)
- Move entries from a group to another
- Create key files
- Open database from URL (and maybe some clouds?)

# Credits
*Dominik Reichl* for the [KeePass application](https://keepass.info/), library and file format  
*David Lechner* for his [PCL adapatation](https://github.com/dlech/KeePass2PCL) of the KeePass Library

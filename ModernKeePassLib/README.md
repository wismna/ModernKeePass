# ModernKeePassLib

This is my adaptation of the KeePassLib (KeePass library) for the Universal Windows Platform and Windows Runtime (WinRT).  
It aims at introducing as little change as possible to the original library: overall, except for namespace changes and some added classes (see below), there is almost no change.  

#Features
- Custom implementation of the System.Security.Cryptography.HashAlgoritm class by using WinRT equivalents
- Use of BouncyCastle PCL to implement AES key derivation features
- Lots of small changes in .NET methods (UTF8 instead of ASCII, string.)
- Disabled native functions (because not compatible with WinRT)
- Use of Splat for GfxUtil
/*
  KeePass Password Safe - The Open-Source Password Manager
  Copyright (C) 2003-2014 Dominik Reichl <dominik.reichl@t-online.de>

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.


  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using Windows.Storage.Streams;
using System.Threading.Tasks;
#if (!ModernKeePassLibPCL && !KeePassLibSD && !KeePassRT)
using System.Net.Cache;
using System.Net.Security;
#endif

#if !ModernKeePassLibPCL && !KeePassRT
using System.Security.Cryptography.X509Certificates;
#endif

#if ModernKeePassLibPCL
using Windows.Storage;
//using PCLStorage;
#endif
using ModernKeePassLibPCL.Utility;

namespace ModernKeePassLibPCL.Serialization
{
#if (!ModernKeePassLibPCL && !KeePassLibSD && !KeePassRT)
	internal sealed class IOWebClient : WebClient
	{
		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest request = base.GetWebRequest(address);
			IOConnection.ConfigureWebRequest(request);
			return request;
		}
	}
#endif

#if !ModernKeePassLibPCL
	internal abstract class WrapperStream : Stream
	{
		private readonly Stream m_s;
		protected Stream BaseStream
		{
			get { return m_s; }
		}

		public override bool CanRead
		{
			get { return m_s.CanRead; }
		}

		public override bool CanSeek
		{
			get { return m_s.CanSeek; }
		}

		public override bool CanTimeout
		{
			get { return m_s.CanTimeout; }
		}

		public override bool CanWrite
		{
			get { return m_s.CanWrite; }
		}

		public override long Length
		{
			get { return m_s.Length; }
		}

		public override long Position
		{
			get { return m_s.Position; }
			set { m_s.Position = value; }
		}

		public override int ReadTimeout
		{
			get { return m_s.ReadTimeout; }
			set { m_s.ReadTimeout = value; }
		}

		public override int WriteTimeout
		{
			get { return m_s.WriteTimeout; }
			set { m_s.WriteTimeout = value; }
		}

		public WrapperStream(Stream sBase) : base()
		{
			if(sBase == null) throw new ArgumentNullException("sBase");

			m_s = sBase;
		}

		public override IAsyncResult BeginRead(byte[] buffer, int offset,
			int count, AsyncCallback callback, object state)
		{
			return m_s.BeginRead(buffer, offset, count, callback, state);
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset,
			int count, AsyncCallback callback, object state)
		{
			return BeginWrite(buffer, offset, count, callback, state);
		}

		public override void Close()
		{
			m_s.Close();
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			return m_s.EndRead(asyncResult);
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			m_s.EndWrite(asyncResult);
		}

		public override void Flush()
		{
			m_s.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return m_s.Read(buffer, offset, count);
		}

		public override int ReadByte()
		{
			return m_s.ReadByte();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return m_s.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			m_s.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			m_s.Write(buffer, offset, count);
		}

		public override void WriteByte(byte value)
		{
			m_s.WriteByte(value);
		}
	}

	internal sealed class IocStream : WrapperStream
	{
		private readonly bool m_bWrite; // Initially opened for writing

		public IocStream(Stream sBase) : base(sBase)
		{
			m_bWrite = sBase.CanWrite;
		}

		public override void Close()
		{
			base.Close();

			if(MonoWorkarounds.IsRequired(10163) && m_bWrite)
			{
				try
				{
					Stream s = this.BaseStream;
					Type t = s.GetType();
					if(t.Name == "WebConnectionStream")
					{
						PropertyInfo pi = t.GetProperty("Request",
							BindingFlags.Instance | BindingFlags.NonPublic);
						if(pi != null)
						{
							WebRequest wr = (pi.GetValue(s, null) as WebRequest);
							if(wr != null)
								IOConnection.DisposeResponse(wr.GetResponse(), false);
							else { Debug.Assert(false); }
						}
						else { Debug.Assert(false); }
					}
				}
				catch(Exception) { Debug.Assert(false); }
			}
		}

		public static Stream WrapIfRequired(Stream s)
		{
			if(s == null) { Debug.Assert(false); return null; }

			if(MonoWorkarounds.IsRequired(10163) && s.CanWrite)
				return new IocStream(s);

			return s;
		}
	}
#endif

	public static class IOConnection
	{
#if (!ModernKeePassLibPCL && !KeePassLibSD && !KeePassRT)
		private static ProxyServerType m_pstProxyType = ProxyServerType.System;
		private static string m_strProxyAddr = string.Empty;
		private static string m_strProxyPort = string.Empty;
		private static string m_strProxyUserName = string.Empty;
		private static string m_strProxyPassword = string.Empty;

		private static bool m_bSslCertsAcceptInvalid = false;
		internal static bool SslCertsAcceptInvalid
		{
			// get { return m_bSslCertsAcceptInvalid; }
			set { m_bSslCertsAcceptInvalid = value; }
		}
#endif

		// Web request methods
		public const string WrmDeleteFile = "DELETEFILE";
		public const string WrmMoveFile = "MOVEFILE";

		// Web request headers
		public const string WrhMoveFileTo = "MoveFileTo";

		public static event EventHandler<IOAccessEventArgs> IOAccessPre;

#if (!ModernKeePassLibPCL && !KeePassLibSD && !KeePassRT)
		// Allow self-signed certificates, expired certificates, etc.
		private static bool AcceptCertificate(object sender,
			X509Certificate certificate, X509Chain chain,
			SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		internal static void SetProxy(ProxyServerType pst, string strAddr,
			string strPort, string strUserName, string strPassword)
		{
			m_pstProxyType = pst;
			m_strProxyAddr = (strAddr ?? string.Empty);
			m_strProxyPort = (strPort ?? string.Empty);
			m_strProxyUserName = (strUserName ?? string.Empty);
			m_strProxyPassword = (strPassword ?? string.Empty);
		}

		internal static void ConfigureWebRequest(WebRequest request)
		{
			if(request == null) { Debug.Assert(false); return; } // No throw

			// WebDAV support
			if(request is HttpWebRequest)
			{
				request.PreAuthenticate = true; // Also auth GET
				if(request.Method == WebRequestMethods.Http.Post)
					request.Method = WebRequestMethods.Http.Put;
			}
			// else if(request is FtpWebRequest)
			// {
			//	Debug.Assert(((FtpWebRequest)request).UsePassive);
			// }

			// Not implemented and ignored in Mono < 2.10
			try
			{
				request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
			}
			catch(NotImplementedException) { }
			catch(Exception) { Debug.Assert(false); }

			try
			{
				IWebProxy prx;
				if(GetWebProxy(out prx)) request.Proxy = prx;
			}
			catch(Exception) { Debug.Assert(false); }
		}

		internal static void ConfigureWebClient(WebClient wc)
		{
			// Not implemented and ignored in Mono < 2.10
			try
			{
				wc.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
			}
			catch(NotImplementedException) { }
			catch(Exception) { Debug.Assert(false); }

			try
			{
				IWebProxy prx;
				if(GetWebProxy(out prx)) wc.Proxy = prx;
			}
			catch(Exception) { Debug.Assert(false); }
		}

		private static bool GetWebProxy(out IWebProxy prx)
		{
			prx = null;

			if(m_pstProxyType == ProxyServerType.None)
				return true; // Use null proxy
			if(m_pstProxyType == ProxyServerType.Manual)
			{
				try
				{
					if(m_strProxyPort.Length > 0)
						prx = new WebProxy(m_strProxyAddr, int.Parse(m_strProxyPort));
					else prx = new WebProxy(m_strProxyAddr);

					if((m_strProxyUserName.Length > 0) || (m_strProxyPassword.Length > 0))
						prx.Credentials = new NetworkCredential(m_strProxyUserName,
							m_strProxyPassword);

					return true; // Use manual proxy
				}
				catch(Exception exProxy)
				{
					string strInfo = m_strProxyAddr;
					if(m_strProxyPort.Length > 0) strInfo += ":" + m_strProxyPort;
					MessageService.ShowWarning(strInfo, exProxy.Message);
				}

				return false; // Use default
			}

			if((m_strProxyUserName.Length == 0) && (m_strProxyPassword.Length == 0))
				return false; // Use default proxy, no auth

			try
			{
				prx = WebRequest.DefaultWebProxy;
				if(prx == null) prx = WebRequest.GetSystemWebProxy();
				if(prx == null) throw new InvalidOperationException();

				prx.Credentials = new NetworkCredential(m_strProxyUserName,
					m_strProxyPassword);
				return true;
			}
			catch(Exception) { Debug.Assert(false); }

			return false;
		}

		private static void PrepareWebAccess()
		{
			if(m_bSslCertsAcceptInvalid)
				ServicePointManager.ServerCertificateValidationCallback =
					IOConnection.AcceptCertificate;
			else
				ServicePointManager.ServerCertificateValidationCallback = null;
		}

		private static IOWebClient CreateWebClient(IOConnectionInfo ioc)
		{
			PrepareWebAccess();

			IOWebClient wc = new IOWebClient();
			ConfigureWebClient(wc);

			if((ioc.UserName.Length > 0) || (ioc.Password.Length > 0))
				wc.Credentials = new NetworkCredential(ioc.UserName, ioc.Password);
			else if(NativeLib.IsUnix()) // Mono requires credentials
				wc.Credentials = new NetworkCredential("anonymous", string.Empty);

			return wc;
		}

		private static WebRequest CreateWebRequest(IOConnectionInfo ioc)
		{
			PrepareWebAccess();

			WebRequest req = WebRequest.Create(ioc.Path);
			ConfigureWebRequest(req);

			if((ioc.UserName.Length > 0) || (ioc.Password.Length > 0))
				req.Credentials = new NetworkCredential(ioc.UserName, ioc.Password);
			else if(NativeLib.IsUnix()) // Mono requires credentials
				req.Credentials = new NetworkCredential("anonymous", string.Empty);

			return req;
		}

		public static Stream OpenRead(IOConnectionInfo ioc)
		{
			RaiseIOAccessPreEvent(ioc, IOAccessType.Read);

			if(StrUtil.IsDataUri(ioc.Path))
			{
				byte[] pbData = StrUtil.DataUriToData(ioc.Path);
				if(pbData != null) return new MemoryStream(pbData, false);
			}

			if(ioc.IsLocalFile()) return OpenReadLocal(ioc);

			return IocStream.WrapIfRequired(CreateWebClient(ioc).OpenRead(
				new Uri(ioc.Path)));
		}
#else
		public static async Task<IRandomAccessStream> OpenRead(IOConnectionInfo ioc)
		{
			RaiseIOAccessPreEvent(ioc, IOAccessType.Read);

			return await OpenReadLocal(ioc);
		}
#endif

		private static async Task<IRandomAccessStream> OpenReadLocal(IOConnectionInfo ioc)
		{
             return await ioc.StorageFile.OpenAsync(FileAccessMode.Read);
		}

#if (!ModernKeePassLibPCL && !KeePassLibSD && !KeePassRT)
		public static Stream OpenWrite(IOConnectionInfo ioc)
		{
			if(ioc == null) { Debug.Assert(false); return null; }

			RaiseIOAccessPreEvent(ioc, IOAccessType.Write);

			if(ioc.IsLocalFile()) return OpenWriteLocal(ioc);

			Uri uri = new Uri(ioc.Path);
			Stream s;

			// Mono does not set HttpWebRequest.Method to POST for writes,
			// so one needs to set the method to PUT explicitly
			if(NativeLib.IsUnix() && (uri.Scheme.Equals(Uri.UriSchemeHttp,
				StrUtil.CaseIgnoreCmp) || uri.Scheme.Equals(Uri.UriSchemeHttps,
				StrUtil.CaseIgnoreCmp)))
				s = CreateWebClient(ioc).OpenWrite(uri, WebRequestMethods.Http.Put);
			else s = CreateWebClient(ioc).OpenWrite(uri);

			return IocStream.WrapIfRequired(s);
		}
#else
		public static async Task<IRandomAccessStream> OpenWrite(IOConnectionInfo ioc)
		{
			RaiseIOAccessPreEvent(ioc, IOAccessType.Write);

			return await OpenWriteLocal(ioc);
		}
#endif

		private static async Task<IRandomAccessStream> OpenWriteLocal(IOConnectionInfo ioc)
		{
            return await ioc.StorageFile.OpenAsync(FileAccessMode.ReadWrite);
        }

		public static bool FileExists(IOConnectionInfo ioc)
		{
			return FileExists(ioc, false);
		}

		public static bool FileExists(IOConnectionInfo ioc, bool bThrowErrors)
		{
			if(ioc == null) { Debug.Assert(false);
			}

			RaiseIOAccessPreEvent(ioc, IOAccessType.Exists);
            
            return ioc.StorageFile.IsAvailable;
		}

		public static async void DeleteFile(IOConnectionInfo ioc)
		{
			RaiseIOAccessPreEvent(ioc, IOAccessType.Delete);
            
		    if (!ioc.IsLocalFile()) return;
		    await ioc.StorageFile?.DeleteAsync();
		}

		/// <summary>
		/// Rename/move a file. For local file system and WebDAV, the
		/// specified file is moved, i.e. the file destination can be
		/// in a different directory/path. In contrast, for FTP the
		/// file is renamed, i.e. its destination must be in the same
		/// directory/path.
		/// </summary>
		/// <param name="iocFrom">Source file path.</param>
		/// <param name="iocTo">Target file path.</param>
		public static async void RenameFile(IOConnectionInfo iocFrom, IOConnectionInfo iocTo)
		{
			RaiseIOAccessPreEvent(iocFrom, iocTo, IOAccessType.Move);
            
		    if (!iocFrom.IsLocalFile()) return;
		    await iocFrom.StorageFile?.RenameAsync(iocTo.Path);
        }

#if (!ModernKeePassLibPCL && !KeePassLibSD && !KeePassRT)
		private static bool SendCommand(IOConnectionInfo ioc, string strMethod)
		{
			try
			{
				WebRequest req = CreateWebRequest(ioc);
				req.Method = strMethod;
				DisposeResponse(req.GetResponse(), true);
			}
			catch(Exception) { return false; }

			return true;
		}
#endif
#if !ModernKeePassLibPCL
        internal static void DisposeResponse(WebResponse wr, bool bGetStream)
		{
			if(wr == null) return;

			try
			{
				if(bGetStream)
				{
					Stream s = wr.GetResponseStream();
					if(s != null) s.Dispose();
				}
			}
			catch(Exception) { Debug.Assert(false); }

			try { wr.Dispose(); }
			catch(Exception) { Debug.Assert(false); }
		}
#endif
        public static async Task<byte[]> ReadFile(IOConnectionInfo ioc)
		{
		    IRandomAccessStream sIn = null;
			MemoryStream ms = null;
			try
			{
				sIn = await OpenRead(ioc);
				if(sIn == null) return null;

				ms = new MemoryStream();
                
				MemUtil.CopyStream(sIn.AsStream(), ms);

				return ms.ToArray();
			}
			catch(Exception) { }
			finally
			{
				if(sIn != null) sIn.Dispose();
				if(ms != null) ms.Dispose();
			}

			return null;
		}

		private static void RaiseIOAccessPreEvent(IOConnectionInfo ioc, IOAccessType t)
		{
			RaiseIOAccessPreEvent(ioc, null, t);
		}

		private static void RaiseIOAccessPreEvent(IOConnectionInfo ioc,
			IOConnectionInfo ioc2, IOAccessType t)
		{
			if(ioc == null) { Debug.Assert(false); return; }
			// ioc2 may be null

			if(IOConnection.IOAccessPre != null)
			{
				IOConnectionInfo ioc2Lcl = ((ioc2 != null) ? ioc2.CloneDeep() : null);
				IOAccessEventArgs e = new IOAccessEventArgs(ioc.CloneDeep(), ioc2Lcl, t);
				IOConnection.IOAccessPre(null, e);
			}
		}
	}
}

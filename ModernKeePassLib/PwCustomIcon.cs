/*
  KeePass Password Safe - The Open-Source Password Manager
  Copyright (C) 2003-2018 Dominik Reichl <dominik.reichl@t-online.de>

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
using System.Collections.Generic;
using System.Diagnostics;
#if ModernKeePassLib
using Image = Splat.IBitmap;
#else
using System.Drawing;
#endif

using ModernKeePassLib.Utility;

namespace ModernKeePassLib
{
	/// <summary>
	/// Custom icon. <c>PwCustomIcon</c> objects are immutable.
	/// </summary>
	public sealed class PwCustomIcon
	{
		private readonly PwUuid m_pwUuid;
		private readonly byte[] m_pbImageDataPng;

		private readonly Image m_imgOrg;
		private Dictionary<long, Image> m_dImageCache = new Dictionary<long, Image>();

		// Recommended maximum sizes, not obligatory
		internal const int MaxWidth = 128;
		internal const int MaxHeight = 128;

		public PwUuid Uuid
		{
			get { return m_pwUuid; }
		}

		public byte[] ImageDataPng
		{
			get { return m_pbImageDataPng; }
		}

		[Obsolete("Use GetImage instead.")]
		public Image Image
		{
#if (!KeePassLibSD && !KeePassUAP)
			get { return GetImage(16, 16); } // Backward compatibility
#else
			get { return GetImage(); } // Backward compatibility
#endif
		}

		public PwCustomIcon(PwUuid pwUuid, byte[] pbImageDataPng)
		{
			Debug.Assert(pwUuid != null);
			if(pwUuid == null) throw new ArgumentNullException("pwUuid");
			Debug.Assert(!pwUuid.Equals(PwUuid.Zero));
			if(pwUuid.Equals(PwUuid.Zero)) throw new ArgumentException("pwUuid == 0.");
			Debug.Assert(pbImageDataPng != null);
			if(pbImageDataPng == null) throw new ArgumentNullException("pbImageDataPng");

			m_pwUuid = pwUuid;
			m_pbImageDataPng = pbImageDataPng;

			// MemoryStream ms = new MemoryStream(m_pbImageDataPng, false);
			// m_imgOrg = Image.FromStream(ms);
			// ms.Close();
#if ModernKeePassLib
            try { m_imgOrg = GfxUtil.LoadImage(m_pbImageDataPng).GetAwaiter().GetResult(); }
#else
			try { m_imgOrg = GfxUtil.LoadImage(m_pbImageDataPng); }
#endif
			catch(Exception) { Debug.Assert(false); m_imgOrg = null; }

			if(m_imgOrg != null)
				m_dImageCache[GetID((int)m_imgOrg.Width, (int)m_imgOrg.Height)] =
					m_imgOrg;
		}

		private static long GetID(int w, int h)
		{
			return (((long)w << 32) ^ (long)h);
		}

		/// <summary>
		/// Get the icon as an <c>Image</c> (original size).
		/// </summary>
		public Image GetImage()
		{
			return m_imgOrg;
		}

#if (!KeePassLibSD && !KeePassUAP)
		/// <summary>
		/// Get the icon as an <c>Image</c> (with the specified size).
		/// </summary>
		/// <param name="w">Width of the returned image.</param>
		/// <param name="h">Height of the returned image.</param>
		public Image GetImage(int w, int h)
		{
			if(w < 0) { Debug.Assert(false); return m_imgOrg; }
			if(h < 0) { Debug.Assert(false); return m_imgOrg; }
			if(m_imgOrg == null) return null;

			long lID = GetID(w, h);

			Image img;
			if(m_dImageCache.TryGetValue(lID, out img)) return img;
#if ModernKeePassLib
            img = GfxUtil.ScaleImage(m_pbImageDataPng, w, h).GetAwaiter().GetResult();
#else
			img = GfxUtil.ScaleImage(m_imgOrg, w, h, ScaleTransformFlags.UIIcon);
#endif
		    m_dImageCache[lID] = img;
		    return img;
		}
#endif
	}
}

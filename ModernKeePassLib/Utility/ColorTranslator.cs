using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace KeePass2PCL.Utility
{
	/// <summary>
	/// Replacement for System.Drawing.ColorTranslator.
	/// </summary>
	/// <remarks>
	/// Colors are stored in the kdbx database file in HTML format (#XXXXXX).
	/// </remarks>
	public static class ColorTranslator
	{
		static Regex longForm = new Regex("^#([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})$");

		/// <summary>
		/// Converts an HTML color value to a Color.
		/// </summary>
		/// <returns>The Color.</returns>
		/// <param name="htmlColor">HTML color code.</param>
		/// <exception cref="ArgumentNullException">If htmlColor is null.</exception>
		/// <exception cref="ArgumentException">If htmlColor did not match the pattern "#XXXXXX".</exception>
		/// <remarks>
		/// Currently only understands "#XXXXXX". "#XXX" or named colors will
		/// throw and exception.
		/// </remarks>
		public static Color FromHtml(string htmlColor)
		{
			if (htmlColor == null)
				throw new ArgumentNullException("htmlColor");
			Match match = longForm.Match(htmlColor);
			if (match.Success) {
				var r = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
				var g = int.Parse(match.Groups[2].Value, NumberStyles.HexNumber);
				var b = int.Parse(match.Groups[3].Value, NumberStyles.HexNumber);
				return Color.FromArgb(r, g, b);
			}
			throw new ArgumentException(string.Format("Could not parse HTML color '{0}'.", htmlColor), "htmlColor");
		}

		/// <summary>
		/// Converts a color to an HTML color code.
		/// </summary>
		/// <returns>String containing the color code.</returns>
		/// <param name="htmlColor">The Color to convert</param>
		/// <remarks>
		/// The string is in the format "#XXXXXX"
		/// </remarks>
		public static string ToHtml(Color htmlColor)
		{
			return string.Format("#{0:x2}{1:x2}{2:x2}", htmlColor.R, htmlColor.G, htmlColor.B);
		}
	}
}


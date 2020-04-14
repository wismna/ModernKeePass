using System.Drawing;
using Windows.UI.Xaml.Media;

namespace ModernKeePass.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToColor(this SolidColorBrush brush)
        {
            return Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
        }

        public static SolidColorBrush ToSolidColorBrush(this Color color)
        {
            return new SolidColorBrush(Windows.UI.Color.FromArgb(color.A, color.R, color.G, color.B));
        }
    }
}
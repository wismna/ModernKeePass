using ModernKeePass.Extensions;
using System;
using System.Drawing;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ModernKeePass.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = value as Color? ?? Color.Empty;
            if (color == Color.Empty && parameter is SolidColorBrush) return (SolidColorBrush) parameter;
            return color.ToSolidColorBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var brush = value as SolidColorBrush;
            return brush?.ToColor() ?? new Color();
        }
    }
}
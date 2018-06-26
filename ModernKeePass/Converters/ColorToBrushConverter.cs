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
            var color = value is Color ? (Color?) value : Color.Empty;
            if (color == Color.Empty && parameter is SolidColorBrush) return (SolidColorBrush) parameter;
            return new SolidColorBrush(Windows.UI.Color.FromArgb(
                color.Value.A,
                color.Value.R,
                color.Value.G,
                color.Value.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var brush = value as SolidColorBrush;
            return brush == null ? new Color() : Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
        }
    }
}
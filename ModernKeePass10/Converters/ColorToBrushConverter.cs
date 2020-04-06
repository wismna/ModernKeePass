using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ModernKeePass.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = value is Color color1 ? color1 : default(Color);
            if (color == default(Color) && parameter is SolidColorBrush) return (SolidColorBrush) parameter;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (value as SolidColorBrush)?.Color ?? new Color();
        }
    }
}
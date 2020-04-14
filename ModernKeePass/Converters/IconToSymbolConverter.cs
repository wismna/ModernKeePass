using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Converters
{
    public class IconToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Enum.Parse(typeof(Symbol), value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Enum.Parse(typeof(Icon), value.ToString());
        }
    }
}
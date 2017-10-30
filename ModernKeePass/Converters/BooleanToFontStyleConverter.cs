using System;
using Windows.UI.Text;
using Windows.UI.Xaml.Data;

namespace ModernKeePass.Converters
{
    public class BooleanToFontStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolean = value is bool ? (bool)value : false;
            return boolean ? FontStyle.Italic : FontStyle.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

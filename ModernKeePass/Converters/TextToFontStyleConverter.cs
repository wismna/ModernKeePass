using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Text;
using Windows.UI.Xaml.Data;

namespace ModernKeePass.Converters
{
    public class TextToFontStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var compareValue = parameter as string;
            var text = value as string;
            return string.Compare(text, compareValue, StringComparison.OrdinalIgnoreCase) == 0
                ? FontStyle.Italic
                : FontStyle.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

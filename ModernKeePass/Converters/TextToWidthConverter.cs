using System;
using Windows.UI.Xaml.Data;

namespace ModernKeePass.Converters
{
    public class TextToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var fontSize = double.Parse(parameter as string);
            var text = value as string;
            return text?.Length * fontSize ?? 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ModernKeePass.Converters
{
    public class EmptyStringToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var text = value is string ? value.ToString() : string.Empty;
            return string.IsNullOrEmpty(text) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

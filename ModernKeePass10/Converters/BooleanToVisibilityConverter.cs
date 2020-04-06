using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ModernKeePass.Converters
{
    [Obsolete]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolean = value as bool? ?? false;
            return boolean ? Visibility.Visible : Visibility.Collapsed;
        }

        // No need to implement this
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = value as Visibility? ?? Visibility.Visible;
            switch (visibility)
            {
                case Visibility.Visible: return true;
                case Visibility.Collapsed: return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }
}

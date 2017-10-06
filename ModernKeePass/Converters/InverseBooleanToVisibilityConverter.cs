using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ModernKeePass.Converters
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boolean = value is bool ? (bool)value : false;
            return boolean ? Visibility.Collapsed : Visibility.Visible;
        }

        // No need to implement this
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = value is Visibility ? (Visibility)value : Visibility.Visible;
            switch (visibility)
            {
                case Visibility.Visible: return false;
                case Visibility.Collapsed: return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace ModernKeePass.Converters
{
    public class PluralizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var pluralizationOptionString = parameter as string;
            var pluralizationOptions = pluralizationOptionString?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (pluralizationOptions == null || pluralizationOptions.Length != 2) return string.Empty;
            var count = value is int ? (int) value : 0;
            var text = count == 1 ? pluralizationOptions[0] : pluralizationOptions[1];
            return $"{count} {text}";
        }

        // No need to implement this
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

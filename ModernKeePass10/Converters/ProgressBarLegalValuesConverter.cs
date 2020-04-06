using System;
using Windows.UI.Xaml.Data;

namespace ModernKeePass.Converters
{
    public class ProgressBarLegalValuesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var legalValuesOptionString = parameter as string;
            var legalValuesOptions = legalValuesOptionString?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (legalValuesOptions == null || legalValuesOptions.Length != 2) return 0;

            var minValue = double.Parse(legalValuesOptions[0]);
            var maxValue = double.Parse(legalValuesOptions[1]);
            var count = value is double ? (double)value : 0;
            if (count > maxValue) return maxValue;
            if (count < minValue) return minValue;
            return count;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

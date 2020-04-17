using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ModernKeePass.Converters
{
    public class DiscreteIntToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = System.Convert.ToInt32(value);
            switch (status)
            {
                case 1: return new SolidColorBrush(Colors.Red);
                case 3: return new SolidColorBrush(Colors.Yellow);
                case 5: return new SolidColorBrush(Colors.Green);
                default: return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

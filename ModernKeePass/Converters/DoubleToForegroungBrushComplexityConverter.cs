using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ModernKeePass.Converters
{
    public class DoubleToForegroungBrushComplexityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var currentValue = (double) value;
                var maxValue = double.Parse(parameter as string);
                var green = System.Convert.ToByte(currentValue / maxValue * byte.MaxValue);
                var red = (byte) (byte.MaxValue - green);
                return new SolidColorBrush(Color.FromArgb(255, red, green, 0));
            }
            catch (OverflowException)
            {
                return new SolidColorBrush(Color.FromArgb(255, 0, byte.MaxValue, 0));
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Media;

namespace ModernKeePass.ViewModels
{
    public class ColorPickerVm
    {
        public struct Color
        {
            public string ColorName { get; set; }
            public SolidColorBrush ColorBrush { get; set; }
        }

        public List<Color> Colors { get; }

        public Color SelectedItem { get; set; }

        public ColorPickerVm()
        {
            Colors = new List<Color>();
            var type = typeof(Windows.UI.Colors);
            var properties = type.GetRuntimeProperties().ToArray();
            foreach (var propertyInfo in properties)
            {
                var color = new Color
                {
                    ColorName = propertyInfo.Name,
                    ColorBrush = new SolidColorBrush((Windows.UI.Color) propertyInfo.GetValue(null, null))
                };
                Colors.Add(color);
                if (color.ColorBrush.Color.Equals(SelectedColor.Color)
            }
        }
    }
}
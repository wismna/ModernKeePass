using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;

namespace ModernKeePass.ViewModels
{
    public class ColorPickerControlVm: ObservableObject
    {
        private Color _selectedItem;

        public sealed class Color
        {
            public string ColorName { get; set; }
            public SolidColorBrush ColorBrush { get; set; }

            public bool Equals(SolidColorBrush color)
            {
                return color.Color.R == ColorBrush.Color.R &&
                       color.Color.G == ColorBrush.Color.G &&
                       color.Color.B == ColorBrush.Color.B &&
                       color.Color.A == ColorBrush.Color.A;
            }
        }

        public List<Color> Colors { get; }

        public Color SelectedItem
        {
            get { return _selectedItem; }
            set { Set(() => SelectedItem, ref _selectedItem, value); }
        }

        public ColorPickerControlVm()
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
            }
        }

        public void Initialize(SolidColorBrush selectedColor)
        {
            SelectedItem = Colors.FirstOrDefault(c => c.Equals(selectedColor));
        }
    }
}
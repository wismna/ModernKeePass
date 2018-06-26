using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class ColorPickerUserControl
    {
        public struct Color
        {
            public string ColorName { get; set; }
            public SolidColorBrush ColorBrush { get; set; }
        }

        public List<Color> Colors { get; }

        public SolidColorBrush SelectedColor
        {
            get { return (SolidColorBrush)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                "SelectedColor",
                typeof(SolidColorBrush),
                typeof(ColorPickerUserControl),
                new PropertyMetadata(new SolidColorBrush(), (o, args) => { }));

        public ColorPickerUserControl()
        {
            InitializeComponent();
            Colors = new List<Color>();
            var type = typeof(Windows.UI.Colors);
            var properties = type.GetRuntimeProperties().ToArray();
            foreach (var propertyInfo in properties)
            {
                Colors.Add(new Color
                {
                    ColorName = propertyInfo.Name,
                    ColorBrush = new SolidColorBrush((Windows.UI.Color)propertyInfo.GetValue(null, null))
                });
            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox.SelectedItem = Colors.Find(c => c.ColorBrush.Color.Equals(SelectedColor.Color));
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ComboBox.SelectedItem as Color? ?? new Color();
            SelectedColor = selectedItem.ColorBrush;
        }
    }
}

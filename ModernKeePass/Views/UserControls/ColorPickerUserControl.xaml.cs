using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ModernKeePass.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class ColorPickerUserControl
    {
        public SolidColorBrush SelectedColor
        {
            get { return (SolidColorBrush)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(SolidColorBrush),
                typeof(ColorPickerUserControl),
                new PropertyMetadata(new SolidColorBrush(), (o, args) =>
                {
                    var colorPickerUserControl = o as ColorPickerUserControl;
                    var vm = colorPickerUserControl?.ComboBox.DataContext as ColorPickerControlVm;
                    vm?.Initialize(args.NewValue as SolidColorBrush);
                }));

        public ColorPickerUserControl()
        {
            InitializeComponent();
        }

        private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Any())
                SelectedColor = (e.AddedItems[0] as ColorPickerControlVm.Color)?.ColorBrush;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class SymbolPickerUserControl
    {
        public IEnumerable<Symbol> Symbols { get; }

        public Symbol SelectedSymbol
        {
            get { return (Symbol)GetValue(SelectedSymbolProperty); }
            set { SetValue(SelectedSymbolProperty, value); }
        }
        public static readonly DependencyProperty SelectedSymbolProperty =
            DependencyProperty.Register(
                "SelectedSymbol",
                typeof(Symbol),
                typeof(SymbolPickerUserControl),
                new PropertyMetadata(Symbol.Stop, (o, args) => { }));
        
        public SymbolPickerUserControl()
        {
            InitializeComponent();
            Symbols = Enum.GetValues(typeof(Symbol)).Cast<Symbol>();
        }

        private void ComboBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            ComboBox.SelectedItem = Symbols.FirstOrDefault(s => s == SelectedSymbol);
        }
    }
}

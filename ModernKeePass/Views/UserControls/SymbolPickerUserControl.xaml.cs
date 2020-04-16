using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Domain.Enums;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class SymbolPickerUserControl
    {
        public List<Symbol> Symbols { get; }

        public Symbol SelectedSymbol
        {
            get { return (Symbol)GetValue(SelectedSymbolProperty); }
            set { SetValue(SelectedSymbolProperty, value); }
        }
        public static readonly DependencyProperty SelectedSymbolProperty =
            DependencyProperty.Register(
                nameof(SelectedSymbol),
                typeof(Symbol),
                typeof(SymbolPickerUserControl),
                new PropertyMetadata(Symbol.Stop, (o, args) => { }));
        
        public SymbolPickerUserControl()
        {
            InitializeComponent();
            Symbols = new List<Symbol>();
            var names = Enum.GetNames(typeof(Icon));
            foreach (var name in names)
            {
                Symbols.Add((Symbol) Enum.Parse(typeof(Symbol), name));
            }
        }

        private void ComboBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            ComboBox.SelectedItem = Symbols.FirstOrDefault(s => s == SelectedSymbol);
        }
    }
}

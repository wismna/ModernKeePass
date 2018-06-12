using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Interfaces;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ModernKeePass.Controls
{
    public class Breadcrumb : Control
    {
        public Breadcrumb()
        {
            DefaultStyleKey = typeof(Breadcrumb);
        }
        public string TargetPage
        {
            get { return (string)GetValue(TargetPageProperty); }
            set { SetValue(TargetPageProperty, value); }
        }

        public static readonly DependencyProperty TargetPageProperty =
            DependencyProperty.Register(
                "TargetPage",
                typeof(string),
                typeof(Breadcrumb),
                new PropertyMetadata(string.Empty, (o, args) => { }));

        public Symbol SeparatorSymbol
        {
            get { return (Symbol)GetValue(SeparatorSymbolProperty); }
            set { SetValue(SeparatorSymbolProperty, value); }
        }

        public static readonly DependencyProperty SeparatorSymbolProperty =
            DependencyProperty.Register(
                "SeparatorSymbol",
                typeof(Symbol),
                typeof(Breadcrumb),
                new PropertyMetadata(Symbol.Forward, (o, args) => { }));

        public IEnumerable<IPwEntity> PathItems
        {
            get { return (IEnumerable<IPwEntity>)GetValue(PathItemsProperty); }
            set { SetValue(PathItemsProperty, value); }
        }

        public static readonly DependencyProperty PathItemsProperty =
            DependencyProperty.Register(
                "PathItems",
                typeof(IEnumerable<>),
                typeof(Breadcrumb),
                new PropertyMetadata(null, (o, args) => { }));
    }
}

using System;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Models
{
    public class MainMenuItem: IIsEnabled
    {
        private string _title;

        public string Title
        {
            get { return IsEnabled ? _title : _title + " - Coming soon"; }
            set { _title = value; }
        }

        public Type PageType { get; set; }
        public object Parameter { get; set; }
        public Frame Destination { get; set; }
        public int Group { get; set; } = 0;
        public bool IsEnabled => PageType != null;

        public override string ToString()
        {
            return Title;
        }
    }
}

﻿using System.Collections.ObjectModel;
using ModernKeePass.Models;
using System.ComponentModel;
using System.Linq;

namespace ModernKeePass.ViewModels
{
    public class MainVm : INotifyPropertyChanged
    {
        public IOrderedEnumerable<IGrouping<int, MainMenuItem>> MainMenuItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

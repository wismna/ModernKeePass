/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ModernKeePass"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator : ViewModelLocatorCommon
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<SettingsVm>();
            SimpleIoc.Default.Register<MainVm>();
            SimpleIoc.Default.Register<GroupsVm>();
            SimpleIoc.Default.Register<EntriesVm>();
            SimpleIoc.Default.Register<GroupItemVm>();
            SimpleIoc.Default.Register<EntryItemVm>();
        }

        public MainVm Main => ServiceLocator.Current.GetInstance<MainVm>(Guid.NewGuid().ToString());
        public SettingsVm Settings => ServiceLocator.Current.GetInstance<SettingsVm>(Guid.NewGuid().ToString());
        public GroupsVm Groups => ServiceLocator.Current.GetInstance<GroupsVm>(Guid.NewGuid().ToString());
        public EntriesVm Entries => ServiceLocator.Current.GetInstance<EntriesVm>(Guid.NewGuid().ToString());
        public GroupItemVm GroupItem => ServiceLocator.Current.GetInstance<GroupItemVm>(Guid.NewGuid().ToString());
        public EntryItemVm EntryItem => ServiceLocator.Current.GetInstance<EntryItemVm>(Guid.NewGuid().ToString());
    }
}
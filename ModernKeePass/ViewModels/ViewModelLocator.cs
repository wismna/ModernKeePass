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
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Domain.Interfaces;
using GalaSoft.MvvmLight;

namespace ModernKeePass.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator: ViewModelLocatorCommon
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                //SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                // Create run time view services and models
                //SimpleIoc.Default.Register<IDataService, DataService>();IDataService
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<IDateTime>());
            }

            SimpleIoc.Default.Register<SettingsVm>();
            SimpleIoc.Default.Register<MainVm>();
            SimpleIoc.Default.Register<GroupDetailVm>();
            SimpleIoc.Default.Register<EntryDetailVm>();
        }
        
        public MainVm Main => ServiceLocator.Current.GetInstance<MainVm>(Guid.NewGuid().ToString());
        public SettingsVm Settings => ServiceLocator.Current.GetInstance<SettingsVm>(Guid.NewGuid().ToString());
        public GroupDetailVm Group => ServiceLocator.Current.GetInstance<GroupDetailVm>(Guid.NewGuid().ToString());
        public EntryDetailVm Entry => ServiceLocator.Current.GetInstance<EntryDetailVm>(Guid.NewGuid().ToString());
    }
}
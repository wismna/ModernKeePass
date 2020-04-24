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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                //SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                // Create run time view services and models
                //SimpleIoc.Default.Register<IDataService, DataService>();IDataService
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<IMediator>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<IRecentProxy>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<IResourceProxy>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<ISettingsProxy>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<ICredentialsProxy>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<IDialogService>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<INavigationService>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<INotificationService>());
            }
            
            SimpleIoc.Default.Register<SettingsVm>();
            SimpleIoc.Default.Register<SettingsDatabaseVm>();
            SimpleIoc.Default.Register<SettingsNewVm>();
            SimpleIoc.Default.Register<SettingsSaveVm>();
            SimpleIoc.Default.Register<SettingsSecurityVm>();
            SimpleIoc.Default.Register<OpenDatabaseControlVm>();
            SimpleIoc.Default.Register<SetCredentialsVm>();
            SimpleIoc.Default.Register<NewVm>();
            SimpleIoc.Default.Register<OpenVm>();
            SimpleIoc.Default.Register<RecentVm>();
            SimpleIoc.Default.Register<SaveVm>();
        }
        
        public MainVm Main => ServiceLocator.Current.GetInstance<MainVm>(Guid.NewGuid().ToString());
        public SettingsVm Settings => ServiceLocator.Current.GetInstance<SettingsVm>(Guid.NewGuid().ToString());
        public SettingsDatabaseVm SettingsDatabase => ServiceLocator.Current.GetInstance<SettingsDatabaseVm>(Guid.NewGuid().ToString());
        public SettingsNewVm SettingsNew => ServiceLocator.Current.GetInstance<SettingsNewVm>(Guid.NewGuid().ToString());
        public SettingsSaveVm SettingsSave => ServiceLocator.Current.GetInstance<SettingsSaveVm>(Guid.NewGuid().ToString());
        public SettingsSecurityVm SettingsSecurity => ServiceLocator.Current.GetInstance<SettingsSecurityVm>(Guid.NewGuid().ToString());
        public OpenDatabaseControlVm OpenDatabaseControl => ServiceLocator.Current.GetInstance<OpenDatabaseControlVm>(Guid.NewGuid().ToString());
        public SetCredentialsVm SetCredentials => ServiceLocator.Current.GetInstance<SetCredentialsVm>(Guid.NewGuid().ToString());
        public NewVm New => ServiceLocator.Current.GetInstance<NewVm>(Guid.NewGuid().ToString());
        public OpenVm Open => ServiceLocator.Current.GetInstance<OpenVm>(Guid.NewGuid().ToString());
        public RecentVm Recent => ServiceLocator.Current.GetInstance<RecentVm>(Guid.NewGuid().ToString());
        public SaveVm Save => ServiceLocator.Current.GetInstance<SaveVm>(Guid.NewGuid().ToString());

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
            SimpleIoc.Default.Reset();
        }
    }
}
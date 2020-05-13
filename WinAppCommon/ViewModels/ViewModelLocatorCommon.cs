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
using ModernKeePass.ViewModels.Settings;

namespace ModernKeePass.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocatorCommon
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocatorCommon()
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
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<IFileProxy>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<ICredentialsProxy>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<IDialogService>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<INavigationService>());
                SimpleIoc.Default.Register(() => App.Services.GetRequiredService<INotificationService>());
            }
            
            SimpleIoc.Default.Register<SecurityVm>();
            SimpleIoc.Default.Register<SettingsNewVm>();
            SimpleIoc.Default.Register<GeneralVm>();
            SimpleIoc.Default.Register<CredentialsVm>();
            SimpleIoc.Default.Register<RecycleBinVm>();
            SimpleIoc.Default.Register<HistoryVm>();
            SimpleIoc.Default.Register<OpenDatabaseControlVm>();
            SimpleIoc.Default.Register<SetCredentialsVm>();
            SimpleIoc.Default.Register<TopMenuVm>();
            SimpleIoc.Default.Register<NewVm>();
            SimpleIoc.Default.Register<OpenVm>();
            SimpleIoc.Default.Register<RecentVm>();
            SimpleIoc.Default.Register<SaveVm>();
        }
        
        public SecurityVm Security => ServiceLocator.Current.GetInstance<SecurityVm>(Guid.NewGuid().ToString());
        public SettingsNewVm SettingsNew => ServiceLocator.Current.GetInstance<SettingsNewVm>(Guid.NewGuid().ToString());
        public GeneralVm General => ServiceLocator.Current.GetInstance<GeneralVm>(Guid.NewGuid().ToString());
        public CredentialsVm Credentials => ServiceLocator.Current.GetInstance<CredentialsVm>(Guid.NewGuid().ToString());
        public RecycleBinVm RecycleBin => ServiceLocator.Current.GetInstance<RecycleBinVm>(Guid.NewGuid().ToString());
        public HistoryVm History => ServiceLocator.Current.GetInstance<HistoryVm>(Guid.NewGuid().ToString());
        public OpenDatabaseControlVm OpenDatabaseControl => ServiceLocator.Current.GetInstance<OpenDatabaseControlVm>(Guid.NewGuid().ToString());
        public SetCredentialsVm SetCredentials => ServiceLocator.Current.GetInstance<SetCredentialsVm>(Guid.NewGuid().ToString());
        public TopMenuVm TopMenu => ServiceLocator.Current.GetInstance<TopMenuVm>(Guid.NewGuid().ToString());
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
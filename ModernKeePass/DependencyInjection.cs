using System.Reflection;
using AutoMapper;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.HockeyApp;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Common;
using ModernKeePass.Views;
using ModernKeePass.Log;

namespace ModernKeePass
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWin81App(this IServiceCollection services)
        {
            var applicationAssembly = typeof(Application.DependencyInjection).GetTypeInfo().Assembly;
            var infrastructureAssembly = typeof(Infrastructure.DependencyInjection).GetTypeInfo().Assembly;
            services.AddAutoMapper(applicationAssembly, infrastructureAssembly);

            services.AddSingleton<INavigationService>(provider =>
            {
                var nav = new NavigationService();
                nav.Configure(Constants.Navigation.MainPage, typeof(MainPage));
                nav.Configure(Constants.Navigation.EntryPage, typeof(EntryDetailPage));
                nav.Configure(Constants.Navigation.GroupPage, typeof(GroupDetailPage));
                return nav;
            });
            services.AddSingleton(provider => Messenger.Default);
            services.AddTransient(typeof(IDialogService), typeof(DialogService));

            services.AddSingleton(provider =>
            {
#if DEBUG
                HockeyClient.Current.Configure("2fe83672-887b-4910-b9de-93a4398d0f8f");
#else
			    HockeyClient.Current.Configure("9eb5fbb79b484fbd8daf04635e975c84");
#endif
                return HockeyClient.Current;
            });
            services.AddSingleton(typeof(ILogger), typeof(HockeyAppLog));

            return services;
        }
    }
}
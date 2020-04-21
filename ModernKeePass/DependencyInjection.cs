using System.Reflection;
using AutoMapper;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.HockeyApp;
using ModernKeePass.Common;
using ModernKeePass.Views;

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

            services.AddSingleton(provider =>
            {
#if DEBUG
                HockeyClient.Current.Configure("2fe83672-887b-4910-b9de-93a4398d0f8f");
#else
			    HockeyClient.Current.Configure("9eb5fbb79b484fbd8daf04635e975c84");
#endif
                return HockeyClient.Current;
            });

            return services;
        }
    }
}
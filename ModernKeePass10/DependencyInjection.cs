using System.Reflection;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using ModernKeePass.Common;
using ModernKeePass.Views;

namespace ModernKeePass
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWin10App(this IServiceCollection services)
        {
            var applicationAssembly = typeof(Application.DependencyInjection).GetTypeInfo().Assembly;
            var infrastructureAssembly = typeof(Infrastructure.DependencyInjection).GetTypeInfo().Assembly;
            services.AddAutoMapper(applicationAssembly, infrastructureAssembly);

            services.AddSingleton<INavigationService>(provider =>
            {
                var nav = new NavigationService();
                nav.Configure(Constants.Navigation.MainPage, typeof(MainPage10));
                nav.Configure(Constants.Navigation.EntryPage, typeof(EntryPage));
                nav.Configure(Constants.Navigation.GroupPage, typeof(EntriesPage));
                return nav;
            });
            services.AddTransient(typeof(IDialogService), typeof(DialogService));

            services.AddSingleton(provider =>
            {

            });

            return services;
        }
    }
}
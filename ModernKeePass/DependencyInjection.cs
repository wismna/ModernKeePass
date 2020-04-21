using System.Reflection;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using Microsoft.Extensions.DependencyInjection;
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

            return services;
        }
    }
}
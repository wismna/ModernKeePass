using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Behaviors;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Common.Services;

namespace ModernKeePass.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).GetTypeInfo().Assembly;
            services.AddMediatR(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DirtyStatusBehavior<,>));

            services.AddSingleton(typeof(IBreadcrumbService), typeof(BreadcrumbService));
            return services;
        }
    }
}
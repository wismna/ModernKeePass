using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ModernKeePass.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).GetTypeInfo().Assembly;
            services.AddAutoMapper(assembly);
            services.AddMediatR(assembly);
            //services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
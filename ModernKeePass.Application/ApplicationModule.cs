using System.Reflection;
using Autofac;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Mappings;
using Module = Autofac.Module;

namespace ModernKeePass.Application
{
    public class ApplicationModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register Automapper profiles
            builder.RegisterType<MappingProfiles>().As<Profile>();

            // Register Mediatr
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            // request & notification handlers
            builder.Register<SingleInstanceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.RegisterAssemblyTypes(typeof(ApplicationModule).GetTypeInfo().Assembly).AsImplementedInterfaces();
        }
    }
}
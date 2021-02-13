using Autofac;
using MediatR;
using System.Reflection;

namespace TicketingSystem.Domain.Application
{
    public class DomainModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder) 
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                IComponentContext componentContext = context.Resolve<IComponentContext>();
                return t => componentContext.Resolve(t);
            });
        }
    }
}

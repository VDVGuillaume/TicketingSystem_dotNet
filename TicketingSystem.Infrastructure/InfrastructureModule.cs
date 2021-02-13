using Autofac;

namespace TicketingSystem.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // builder.RegisterAssemblyTypes(typeof(...CommandHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
            // builder.RegisterAssemblyTypes(typeof(...QueryHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        }
    }
}

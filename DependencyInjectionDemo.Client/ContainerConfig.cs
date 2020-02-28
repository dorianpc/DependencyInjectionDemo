using Autofac;

namespace DependencyInjectionDemo.Client
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DataAccessModule>();
            return containerBuilder.Build();
        }
    }
}
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using DependencyInjectionDemo.DataAccess;


namespace DependencyInjectionDemo.Client
{
    internal class DataAccessModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataAcccessTypes = Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                   .Where(fileName => fileName.Contains("DependencyInjectionDemo") && fileName.EndsWith("DataAccess.dll"))
                   .Select(filePath => Assembly.LoadFrom(filePath))
                   .SelectMany(assembly => assembly.GetTypes()
                       .Where(type => typeof(IDataAccess).IsAssignableFrom(type) && type.IsClass));

            foreach (var dataAccessType in dataAcccessTypes)
            {
                builder.RegisterType(dataAccessType).As<IDataAccess>();
            }
        }

    }
}
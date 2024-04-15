using System.Reflection;
using BugHunters.Api.Common.HandlerContract;

namespace BugHunters.Api.Common.FunctionalCore;

public static class CoreServiceExtensions
{
    public static void RegisterCoreServices(this IServiceCollection services)
    {
        Assembly? assembly = Assembly.GetAssembly(typeof(Program));
        if (assembly is null)
        {
            throw new InvalidOperationException("Could not find the assembly.");
        }

        List<Type> coreServices = assembly.GetTypes()
            .Where(t => !t.IsAbstract)
            .Where(t => !t.IsInterface)
            .Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom( typeof(ICoreService))))
            .ToList();

        foreach (Type serviceType in coreServices)
        {
            services.AddScoped(serviceType);
        }
    }
}
using System.Reflection;

namespace BugHunters.Api.Common.HandlerContract;

public static class HandlerExtensions
{
    public static void RegisterCommandHandlers(this IServiceCollection services)
    {
        Assembly? assembly = Assembly.GetAssembly(typeof(Program));
        if (assembly is null)
        {
            throw new InvalidOperationException("Could not find the assembly.");
        }

        RegisterCommandHandlers(services, assembly);
        RegisterQueryHandlers(services, assembly);
    }

    private static void RegisterQueryHandlers(IServiceCollection services, Assembly assembly)
        => assembly.GetTypes()
            .Where(t => !t.IsAbstract)
            .Where(t => !t.IsInterface)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            .ToList().ForEach(handlerType =>
            {
                Type interfaceType = handlerType.GetInterfaces().Single();
                services.AddScoped(interfaceType, handlerType);
            });

    private static void RegisterCommandHandlers(IServiceCollection services, Assembly assembly)
        => assembly.GetTypes()
            .Where(t => !t.IsAbstract)
            .Where(t => !t.IsInterface)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
            .ToList().ForEach(handlerType =>
            {
                Type interfaceType = handlerType.GetInterfaces().Single();
                services.AddScoped(interfaceType, handlerType);
            });
}
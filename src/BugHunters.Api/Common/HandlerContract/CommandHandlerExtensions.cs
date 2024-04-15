using System.Reflection;

namespace BugHunters.Api.Common.HandlerContract;

public static class CommandHandlerExtensions
{
    public static void RegisterCommandHandlers(this IServiceCollection services)
    {
        Assembly? assembly = Assembly.GetAssembly(typeof(Program));
        if (assembly is null)
        {
            throw new InvalidOperationException("Could not find the assembly.");
        }

        List<Type> handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract)
            .Where(t => !t.IsInterface)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
            .ToList();

        foreach (Type handlerType in handlerTypes)
        {
            Type interfaceType = handlerType.GetInterfaces().Single();
            services.AddScoped(interfaceType, handlerType);
        }
    }
}
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace YnabProgressConsole.Commands;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCommands(this IServiceCollection serviceCollection)
    {
        var commandsAssembly = Assembly.GetAssembly(typeof(ICommand));
        
        return serviceCollection
            .AddMediatRCommandsAndHandlers(commandsAssembly)
            .AddCommandGenerators(commandsAssembly);
    }

    private static IServiceCollection AddMediatRCommandsAndHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        => serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

    private static IServiceCollection AddCommandGenerators(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var implementationTypes = assembly.GetTypes()
            .Where(anyType => anyType.IsClass && typeof(ICommandGenerator).IsAssignableFrom(anyType))
            .ToList();
        
        foreach (var implementationType in implementationTypes)
        {
            var genericInterfaceType = implementationType
                .GetInterfaces()
                .FirstOrDefault(interfaceType => interfaceType.GenericTypeArguments.Length != 0);

            if (genericInterfaceType is null)
            {
                var implementationTypeName = implementationType.Name;
                var typeName = typeof(ITypedCommandGenerator<>).Name;
                
                throw new ArgumentException($"Type '{implementationTypeName}' does not implement {typeName} interface");
            }
            
            var typeForAssignedCommand = genericInterfaceType.GenericTypeArguments.First();

            var commandNameField = typeForAssignedCommand
                .GetFields()
                .FirstOrDefault(lol => lol.Name == "CommandName");
            
            var commandNameValue = commandNameField.GetValue(typeForAssignedCommand);
            
            serviceCollection.AddKeyedSingleton(
                typeof(ICommandGenerator),
                commandNameValue,
                implementationType);
        }
        
        return serviceCollection;
    }
}
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using YnabProgressConsole.Commands.CommandList;
using YnabProgressConsole.Commands.RecurringTransactions;

namespace YnabProgressConsole.Commands;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCommands(this IServiceCollection serviceCollection)
    {
        var commandsAssembly = Assembly.GetAssembly(typeof(ICommand));
        if (commandsAssembly == null)
        {
            throw new NullReferenceException("No Assembly Containing ICommand Implementation");
        }
        
        return serviceCollection
            .AddMediatRCommandsAndHandlers(commandsAssembly)
            .AddCommandGenerators(commandsAssembly);
    }

    private static IServiceCollection AddMediatRCommandsAndHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        => serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

    private static IServiceCollection AddCommandGenerators(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var implementationTypes = assembly.WhereClassTypesImplementType(typeof(ICommandGenerator));
        
        foreach (var implementationType in implementationTypes)
        {
            var genericInterfaceType = implementationType.GetRequiredFirstGenericInterface();
            
            var typeForReferencedCommand = genericInterfaceType.GenericTypeArguments.First();

            var commandNameField = typeForReferencedCommand.GetRequiredField(nameof(CommandListCommand.CommandName));
            var alternateCommandNamesField = typeForReferencedCommand.GetRequiredField(nameof(CommandListCommand.ShorthandCommandName));
            
            var commandNameValue = commandNameField.GetValue(typeForReferencedCommand);
            var shorthandCommandName = alternateCommandNamesField.GetValue(typeForReferencedCommand);

            serviceCollection
                .AddKeyedSingleton(
                    typeof(ICommandGenerator),
                    commandNameValue,
                    implementationType)
                .AddKeyedSingleton(
                    typeof(ICommandGenerator),
                    shorthandCommandName,
                    implementationType);
        }
        
        return serviceCollection;
    }
}
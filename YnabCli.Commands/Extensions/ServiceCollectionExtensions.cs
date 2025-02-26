using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using YnabCli.Commands.Builders;
using YnabCli.Commands.Factories;
using YnabCli.Commands.Generators;

namespace YnabCli.Commands.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<BudgetsClientFactory>()
            .AddSingleton<CommandHelpViewModelBuilder>();
    }

    public static IServiceCollection AddMediatRCommandsAndHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        => serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

    public static IServiceCollection AddCommandGenerators(this IServiceCollection serviceCollection, Assembly assembly)
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
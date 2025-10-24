using System.Reflection;
using Cli.Commands.Abstractions;
using Cli.Instructions;
using Microsoft.Extensions.DependencyInjection;
using YnabCli.Commands.Builders;
using YnabCli.Commands.Generators;
using YnabCli.Database;

namespace YnabCli.Commands.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<ConfiguredBudgetClient>()
            .AddSingleton<CommandHelpCliTableBuilder>();
    }
    
    public static IServiceCollection AddCommandsFromAssembly(this IServiceCollection serviceCollection, Assembly? assembly) 
    {
        if (assembly == null)
        {
            throw new NullReferenceException("No Assembly Containing ICommand Implementation");
        }

        return serviceCollection
            .AddCommandGenerators(assembly)
            .AddMediatRCommandsAndHandlers(assembly);
    }

    public static IServiceCollection AddMediatRCommandsAndHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        => serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

    public static IServiceCollection AddCommandGenerators(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var implementationTypes = assembly.WhereClassTypesImplementType(typeof(IGenericCommandGenerator));
        
        foreach (var implementationType in implementationTypes)
        {
            var genericInterfaceType = implementationType.GetRequiredFirstGenericInterface();
            
            var typeForReferencedCommand = genericInterfaceType.GenericTypeArguments.First();
            
            var name = typeForReferencedCommand.Name.Replace("Command", string.Empty);

            var commandName = name.ToLowerSplitString(ConsoleInstructionConstants.DefaultCommandNameSeparator);
            var shorthandCommandName = name.ToLowerTitleCharacters();

            serviceCollection
                .AddKeyedSingleton(
                    typeof(IGenericCommandGenerator),
                    commandName,
                    implementationType)
                .AddKeyedSingleton(
                    typeof(IGenericCommandGenerator),
                    shorthandCommandName,
                    implementationType);
        }
        
        return serviceCollection;
    }
}
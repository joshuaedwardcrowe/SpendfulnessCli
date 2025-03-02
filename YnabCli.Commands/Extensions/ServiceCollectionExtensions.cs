using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using YnabCli.Commands.Builders;
using YnabCli.Commands.Generators;
using YnabCli.Database;
using YnabCli.Instructions;

namespace YnabCli.Commands.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<DbBudgetClient>()
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
            
            var name = typeForReferencedCommand.Name.Replace("Command", string.Empty);

            var commandName = name.ToLowerSplitString(InstructionConstants.DefaultCommandNameSeparator);
            var shorthandCommandName = name.ToLowerTitleCharacters();
            
            serviceCollection
                .AddKeyedSingleton(
                    typeof(ICommandGenerator),
                    commandName,
                    implementationType)
                .AddKeyedSingleton(
                    typeof(ICommandGenerator),
                    shorthandCommandName,
                    implementationType);
        }
        
        return serviceCollection;
    }
}
using System.Reflection;
using Cli.Instructions.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Commands.Abstractions.Extensions;

public static class ServiceCollectionExtensions
{
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
            
            // TODO: This could really do with not being hardcoded.
            var name = typeForReferencedCommand.Name.Replace("CliCommand", string.Empty);
            
            var commandName = name.ToLowerSplitString(CliInstructionConstants.DefaultCommandNameSeparator);
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
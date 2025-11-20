using System.Reflection;
using Cli.Commands.Abstractions.Generators;
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
            .AddCommandProperties()
            .AddCommandGenerators(assembly)
            .AddMediatRCommandsAndHandlers(assembly);
    }

    private static IServiceCollection AddMediatRCommandsAndHandlers(this IServiceCollection serviceCollection, Assembly assembly)
        => serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

    private static IServiceCollection AddCommandGenerators(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var implementationTypes = assembly.WhereClassTypesImplementType(typeof(IUnidentifiedCliCommandGenerator));
        
        foreach (var implementationType in implementationTypes)
        {
            var genericInterfaceType = implementationType.GetRequiredFirstGenericInterface();
            
            var typeForReferencedCommand = genericInterfaceType.GenericTypeArguments.First();
            
            var name = typeForReferencedCommand.Name.Replace(nameof(CliCommand), string.Empty);
            
            var commandName = name.ToLowerSplitString(CliInstructionConstants.DefaultCommandNameSeparator);
            var shorthandCommandName = name.ToLowerTitleCharacters();

            serviceCollection
                .AddKeyedSingleton(
                    typeof(IUnidentifiedCliCommandGenerator),
                    commandName,
                    implementationType)
                .AddKeyedSingleton(
                    typeof(IUnidentifiedCliCommandGenerator),
                    shorthandCommandName,
                    implementationType);
        }
        
        return serviceCollection;
    }
}
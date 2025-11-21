using System.Reflection;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Generators;
using Cli.Instructions.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Commands.Abstractions.Extensions;

public static class CommandServiceCollectionExtensions
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
            var generatorCommandType = implementationType.FirstOrDefaultGenericTypeArgument();
            var generatorAttribute = implementationType.FirstOrDefaultAttributeOfType<CliCommandGeneratorFor>();

            var fullCommandName = generatorAttribute != null
                ? generatorAttribute.CommandType.Name
                : generatorCommandType.Name;
            
            var specificCommandName = CliCommand.StripInstructionName(fullCommandName);
            
            var commandName = specificCommandName.ToLowerSplitString(CliInstructionConstants.DefaultCommandNameSeparator);
            var shorthandCommandName = specificCommandName.ToLowerTitleCharacters();

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
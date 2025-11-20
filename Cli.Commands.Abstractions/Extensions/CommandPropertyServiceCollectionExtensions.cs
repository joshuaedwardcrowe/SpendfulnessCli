using System.Reflection;
using Cli.Abstractions;
using Cli.Commands.Abstractions.Properties;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Commands.Abstractions.Extensions;

public static class CommandPropertyServiceCollectionExtensions
{
    public static IServiceCollection AddCommandProperties(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<ICliCommandPropertyFactory, MessageCliCommandPropertyFactory>();
    }
    
    public static IServiceCollection AddAggregatorCommandPropertiesFromAssembly(this IServiceCollection serviceCollection, Assembly? assembly)
    {
        if (assembly == null)
        {
            throw new ArgumentNullException(nameof(assembly), "No Assembly Containing ICommand Implementation");
        }
        
        var possibleAggregatorTypes = assembly
            .GetTypes()
            .Where(type => !type.IsAbstract && type.BaseType != typeof(object));
        
        foreach (var possibleAggregatorType in possibleAggregatorTypes)
        {
            var aggregatorType = possibleAggregatorType.GetSuperclassGenericOf(typeof(CliAggregator<>));
            if (aggregatorType is null)
            {
                continue;
            }
            
            var typeForReferencedAggregate = aggregatorType.GenericTypeArguments.First();
        
            var strategyType = typeof(AggregatorCliCommandPropertyFactory<>).MakeGenericType(typeForReferencedAggregate);

            var instance = Activator.CreateInstance(strategyType);
            if (instance is not ICliCommandPropertyFactory factoryInstance)
            {
                throw new InvalidOperationException(
                    $"Could not create instance of type {strategyType.Name} as ICliCommandPropertyFactory");
            }

            serviceCollection.AddSingleton(factoryInstance);
        }
        
        return serviceCollection;
    }
}
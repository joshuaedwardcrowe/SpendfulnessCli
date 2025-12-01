using System.Reflection;
using Cli.Abstractions;
using Cli.Abstractions.Aggregators;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Artefacts.Aggregator;
using Cli.Commands.Abstractions.Artefacts.Aggregator.Filters;
using Cli.Commands.Abstractions.Artefacts.CommandRan;
using Cli.Commands.Abstractions.Artefacts.Page;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Commands.Abstractions.Extensions;

public static class CommandArtefactServiceCollectionExtensions
{
    public static IServiceCollection AddCommandProperties(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<ICliCommandArtefactFactory, RanCliCommandArtefactFactory>()
            .AddSingleton<ICliCommandArtefactFactory, PageSizeCliCommandArtefactFactory>()
            .AddSingleton<ICliCommandArtefactFactory, PageNumberCliCommandArtefactFactory>()
            .AddSingleton<ICliCommandArtefactFactory, CliListAggregatorFilterCliCommandArtefactFactory>();
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
        
            var strategyType = typeof(AggregatorCliCommandArtefactFactory<>).MakeGenericType(typeForReferencedAggregate);

            var instance = Activator.CreateInstance(strategyType);
            if (instance is not ICliCommandArtefactFactory factoryInstance)
            {
                throw new InvalidOperationException(
                    $"Could not create instance of type {strategyType.Name} as ICliCommandPropertyFactory");
            }

            serviceCollection.AddSingleton(factoryInstance);
        }
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddListAggregatorCommandPropertiesFromAssembly(this IServiceCollection serviceCollection, Assembly? assembly)
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
            var aggregatorType = possibleAggregatorType.GetSuperclassGenericOf(typeof(CliListAggregator<>));
            if (aggregatorType is null)
            {
                continue;
            }
            
            var typeForReferencedAggregate = aggregatorType.GenericTypeArguments.First();
        
            var strategyType = typeof(ListAggregatorCliCommandArtefactFactory<>).MakeGenericType(typeForReferencedAggregate);

            var instance = Activator.CreateInstance(strategyType);
            if (instance is not ICliCommandArtefactFactory factoryInstance)
            {
                throw new InvalidOperationException(
                    $"Could not create instance of type {strategyType.Name} as ICliCommandPropertyFactory");
            }

            serviceCollection.AddSingleton(factoryInstance);
        }
        
        return serviceCollection;
    }
}
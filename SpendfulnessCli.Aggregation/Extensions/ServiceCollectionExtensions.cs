using Cli.Commands.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SpendfulnessCli.Aggregation.Aggregator;

namespace SpendfulnessCli.Aggregation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAggregatorCommandProperties(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddAggregatorCommandPropertiesFromAssembly(typeof(YnabAggregator<>).Assembly);
}
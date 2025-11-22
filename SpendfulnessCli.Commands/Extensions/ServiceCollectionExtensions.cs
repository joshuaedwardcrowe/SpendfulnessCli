using Cli.Commands.Abstractions.Properties;
using Spendfulness.Database;
using Microsoft.Extensions.DependencyInjection;

namespace SpendfulnessCli.Commands.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<SpendfulnessBudgetClient>()
            .AddSingleton<ICliCommandPropertyFactory, AccountCliCommandPropertyFactory>();
    }
}
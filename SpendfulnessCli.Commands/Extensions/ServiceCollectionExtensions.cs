using Spendfulness.Database;
using Microsoft.Extensions.DependencyInjection;
using SpendfulnessCli.Commands.Builders;

namespace SpendfulnessCli.Commands.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<SpendfulnessBudgetClient>()
            .AddSingleton<CommandHelpCliTableBuilder>();
    }
}
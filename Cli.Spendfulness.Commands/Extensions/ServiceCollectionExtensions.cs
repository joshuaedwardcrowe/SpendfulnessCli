using Cli.Spendfulness.Commands.Builders;
using Cli.Spendfulness.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Spendfulness.Commands.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<ConfiguredBudgetClient>()
            .AddSingleton<CommandHelpCliTableBuilder>();
    }
}
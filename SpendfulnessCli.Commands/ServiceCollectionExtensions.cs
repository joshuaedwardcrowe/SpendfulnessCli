using Cli.Commands.Abstractions.Artefacts;
using Microsoft.Extensions.DependencyInjection;
using Spendfulness.Database;
using SpendfulnessCli.Commands.Accounts;

namespace SpendfulnessCli.Commands;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpendfulnessCommands(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<SpendfulnessBudgetClient>()
            .AddSingleton<ICliCommandArtefactFactory, AccountCliCommandArtefactFactory>();
    }
}
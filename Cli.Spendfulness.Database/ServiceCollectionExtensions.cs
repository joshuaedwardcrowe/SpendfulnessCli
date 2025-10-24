using Microsoft.Extensions.DependencyInjection;

namespace Cli.Spendfulness.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDb(this IServiceCollection services)
        => services
            .AddDbContext<YnabCliDbContext>()
            .AddSingleton<YnabCliDb>()
            .AddSingleton<ConfiguredBudgetClient>();
}
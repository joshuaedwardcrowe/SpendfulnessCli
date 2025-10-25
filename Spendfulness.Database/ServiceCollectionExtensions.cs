using Microsoft.Extensions.DependencyInjection;

namespace Spendfulness.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpendfulnessDb(this IServiceCollection services)
        => services
            .AddDbContext<SpendfulnessDbContext>()
            .AddSingleton<SpendfulnessDb>()
            .AddSingleton<SpendfulnessBudgetClient>();
}
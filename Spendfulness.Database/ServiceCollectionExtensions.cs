using Microsoft.Extensions.DependencyInjection;
using Spendfulness.Database.Accounts;
using Spendfulness.Database.Users;

namespace Spendfulness.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpendfulnessDb(this IServiceCollection services)
        => services
            .AddDbContext<SpendfulnessDbContext>()
            .AddSingleton<SpendfulnessDb>()
            .AddYnabWrappers()
            .AddAccountRepositories()
            .AddUserRepositories();

    private static IServiceCollection AddYnabWrappers(this IServiceCollection services)
        => services
            .AddSingleton<SpendfulnessBudgetClient>();

    private static IServiceCollection AddAccountRepositories(this IServiceCollection services)
        => services
            .AddSingleton<CustomAccountAttributeRepository>()
            .AddSingleton<CustomAccountTypeRepository>();

    private static IServiceCollection AddUserRepositories(this IServiceCollection services)
        => services
            .AddSingleton<UserRepository>();
}
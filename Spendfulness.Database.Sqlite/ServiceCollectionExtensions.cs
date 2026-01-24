using Microsoft.Extensions.DependencyInjection;
using Spendfulness.Database.Sqlite.Accounts;
using Spendfulness.Database.Sqlite.Users;

namespace Spendfulness.Database.Sqlite;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpendfulnessDb(this IServiceCollection services)
        => services
            .AddDbContext<SpendfulnessDbContext>()
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
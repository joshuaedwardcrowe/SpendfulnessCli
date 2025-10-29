using System.Reflection;
using Cli.Commands.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SpendfulnessCli.Commands.Personalisation.Accounts.Identify.ChangeStrategies;
using SpendfulnessCli.Commands.Personalisation.Databases;

namespace SpendfulnessCli.Commands.Personalisation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersonalisationCommands(this IServiceCollection serviceCollection)
    {
        var personalisationCommandsAssembly = Assembly.GetAssembly(typeof(DatabaseCliCommand));

        return serviceCollection
            .AddAccountAttributeChangeStrategies()
            .AddCommandsFromAssembly(personalisationCommandsAssembly);
    }

    private static IServiceCollection AddAccountAttributeChangeStrategies(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<IAccountAttributeChangeStrategy, CustomAccountTypeChangeStrategy>()
            .AddSingleton<IAccountAttributeChangeStrategy, InterestRateChangeStrategy>();
}
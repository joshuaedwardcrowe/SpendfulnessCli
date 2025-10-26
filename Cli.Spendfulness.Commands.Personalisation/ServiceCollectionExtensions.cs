using System.Reflection;
using Cli.Commands.Abstractions.Extensions;
using Cli.Spendfulness.Commands.Personalisation.Accounts.Identify;
using Cli.Spendfulness.Commands.Personalisation.Databases;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Spendfulness.Commands.Personalisation;

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
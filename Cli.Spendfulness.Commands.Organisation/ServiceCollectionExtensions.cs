using System.Reflection;
using Cli.Commands.Abstractions.Extensions;
using Cli.Spendfulness.Commands.Organisation.CopyOnBudget;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Spendfulness.Commands.Organisation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrganisationCommands(this IServiceCollection serviceCollection)
    {
        var organisationCommandsAssembly = Assembly.GetAssembly(typeof(CopyOnBudgetCliCommand));
        return serviceCollection.AddCommandsFromAssembly(organisationCommandsAssembly);
    }
}
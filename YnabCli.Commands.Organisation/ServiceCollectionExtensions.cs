using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using YnabCli.Commands.Extensions;
using YnabCli.Commands.Organisation.MoveOnBudget;

namespace YnabCli.Commands.Organisation;

public static class ServiceCollectionExtensions
{
    // TODO: This is repetitive, create an .AddReportingCommands extension.
    public static IServiceCollection AddOrganisationCommands(this IServiceCollection serviceCollection)
    {
        var commandsAssembly = Assembly.GetAssembly(typeof(CopyOnBudgetCommand));
        if (commandsAssembly == null)
        {
            throw new NullReferenceException("No Assembly Containing ICommand Implementation");
        }

        return serviceCollection
            .AddCommandGenerators(commandsAssembly)
            .AddMediatRCommandsAndHandlers(commandsAssembly);
    }
}
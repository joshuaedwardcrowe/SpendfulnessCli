using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using YnabCli.Commands.Extensions;
using YnabCli.Commands.Reporting.SpareMoney;

namespace YnabCli.Commands.Reporting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReportingCommands(this IServiceCollection serviceCollection)
    {
        var commandsAssembly = Assembly.GetAssembly(typeof(SpareMoneyCommand));
        if (commandsAssembly == null)
        {
            throw new NullReferenceException("No Assembly Containing ICommand Implementation");
        }

        return serviceCollection
            .AddCommandGenerators(commandsAssembly)
            .AddMediatRCommandsAndHandlers(commandsAssembly);
    }
}
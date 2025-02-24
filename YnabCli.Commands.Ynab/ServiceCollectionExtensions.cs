using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using YnabCli.Commands.Ynab.SpareMoney;

namespace YnabCli.Commands.Ynab;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYnabCliYnabCommands(this IServiceCollection serviceCollection)
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
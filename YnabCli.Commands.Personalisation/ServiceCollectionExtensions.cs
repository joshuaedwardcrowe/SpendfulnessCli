using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using YnabCli.Commands.Personalisation.Databases;

namespace YnabCli.Commands.Personalisation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYnabCliDatabaseCommands(this IServiceCollection serviceCollection)
    {
        var commandsAssembly = Assembly.GetAssembly(typeof(DatabaseCommand));
        if (commandsAssembly == null)
        {
            throw new NullReferenceException("No Assembly Containing ICommand Implementation");
        }

        return serviceCollection
            .AddCommandGenerators(commandsAssembly)
            .AddMediatRCommandsAndHandlers(commandsAssembly);
    }
}
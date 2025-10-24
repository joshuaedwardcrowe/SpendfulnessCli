using System.Reflection;
using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Extensions;
using Cli.Spendfulness.Commands.Personalisation.Databases;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Spendfulness.Commands.Personalisation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersonalisationCommands(this IServiceCollection serviceCollection)
    {
        var personalisationCommandsAssembly = Assembly.GetAssembly(typeof(DatabaseCommand));
        return serviceCollection.AddCommandsFromAssembly(personalisationCommandsAssembly);
    }
}
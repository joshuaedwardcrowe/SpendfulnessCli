using System.Reflection;
using Cli.Commands.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SpendfulnessCli.Commands.Chat.Chat;

namespace SpendfulnessCli.Commands.Chat;

public static class SpendfulnessChatCommandsServiceCollectionExtensions
{
    public static IServiceCollection AddSpendfulnessChatCommands(this IServiceCollection serviceCollection)
    {
        var chatCommandsAssembly = Assembly.GetAssembly(typeof(ChatCliCommand));
        return serviceCollection.AddCommandsFromAssembly(chatCommandsAssembly);
    }
}
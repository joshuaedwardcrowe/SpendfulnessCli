using Microsoft.Extensions.DependencyInjection;
using YnabProgressConsole.Commands;
using YnabProgressConsole.Commands.CommandList;
using YnabProgressConsole.Commands.RecurringTransactions;
using YnabProgressConsole.ViewModels;

namespace YnabProgressConsole.Extensions;

public static class ServiceCollectionExtensions
{
    // TODO: Wonder if this could scan for all implementations of ICommandGenerator.
    public static IServiceCollection AddCommandGenerators(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddKeyedSingleton<ICommandGenerator, CommandListCommandGenerator>(
                CommandListCommandGenerator.CommandName)
            .AddKeyedSingleton<ICommandGenerator, RecurringTransactionsCommandGenerator>(
                RecurringTransactionsCommand.CommandName);

    public static IServiceCollection AddViewModelConstructors(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<RecurringTransactionsViewModelConstructor>();
}
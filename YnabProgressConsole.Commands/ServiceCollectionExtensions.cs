using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using YnabProgressConsole.Commands.CommandList;
using YnabProgressConsole.Commands.RecurringTransactions;
using YnabProgressConsole.Commands.SalaryIncreases;
using YnabProgressConsole.Commands.SpareMoney;

namespace YnabProgressConsole.Commands;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCommands(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddCommandGenerators();

    // TODO: Wonder if this could scan for all implementations of ICommandGenerator.
    private static IServiceCollection AddCommandGenerators(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddKeyedSingleton<ICommandGenerator, CommandListCommandGenerator>(
                CommandListCommandGenerator.CommandName)
            .AddKeyedSingleton<ICommandGenerator, RecurringTransactionsCommandGenerator>(
                RecurringTransactionsCommand.CommandName)
            .AddKeyedSingleton<ICommandGenerator, SalaryIncreasesCommandGenerator>(
                SalaryIncreasesCommand.CommandName)
            .AddKeyedSingleton<ICommandGenerator, SpareMoney.SpareMoneyCommandGenerator>(
                SpareMoneyCommand.CommandName);
}
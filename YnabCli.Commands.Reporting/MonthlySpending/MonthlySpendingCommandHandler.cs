using Cli.Commands.Abstractions;
using Cli.Outcomes;
using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Abstractions;
using YnabCli.Aggregation.Aggregates;
using YnabCli.Aggregation.Aggregator.ListAggregators;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.MonthlySpending;

public class MonthlySpendingCommandHandler: CommandHandler, ICommandHandler<MonthlySpendingCommand>
{
    private readonly ConfiguredBudgetClient _configuredBudgetClient;
    private readonly TransactionMonthChangeCliTableBuilder _transactionMonthChangeCliTableBuilder;

    public MonthlySpendingCommandHandler(ConfiguredBudgetClient configuredBudgetClient, TransactionMonthChangeCliTableBuilder transactionMonthChangeCliTableBuilder)
    {
        _configuredBudgetClient = configuredBudgetClient;
        _transactionMonthChangeCliTableBuilder = transactionMonthChangeCliTableBuilder;
    }

    public async Task<CliCommandOutcome> Handle(MonthlySpendingCommand command, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(command);

        var viewModel = _transactionMonthChangeCliTableBuilder
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }

    private async Task<ListYnabAggregator<TransactionMonthTotalAggregate>> PrepareAggregator(MonthlySpendingCommand command)
    {
        var budget = await _configuredBudgetClient.GetDefaultBudget();

        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionMonthTotalYnabAggregator(transactions);

        if (command.CategoryId.HasValue)
        {
            aggregator.BeforeAggregation(o => o.FilterToCategories(command.CategoryId.Value));
        }
        
        return aggregator;
    }
}
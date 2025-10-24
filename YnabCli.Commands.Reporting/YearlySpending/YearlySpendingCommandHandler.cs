using Cli.Commands.Abstractions;
using Cli.Outcomes;
using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Abstractions;
using YnabCli.Aggregation.Aggregator;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.YearlySpending;

public class YearlySpendingCommandHandler(ConfiguredBudgetClient budgetClient)
    : CommandHandler, ICommandHandler<YearlySpendingCommand>
{
    public async Task<CliCommandOutcome> Handle(YearlySpendingCommand request, CancellationToken cancellationToken)
    {
        var budget =  await budgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();

        var aggregator = new CategoryYearAverageYnabAggregator(transactions)
            .BeforeAggregation(t => t.FilterOutTransfers())
            .BeforeAggregation(t => t.FilterOutAutomations());

        var viewModel = new CategoryYearAverageCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
}
using Cli.Commands.Abstractions;
using Cli.Outcomes;
using ConsoleTables;
using Ynab;
using Ynab.Extensions;
using YnabCli.Abstractions;
using YnabCli.Aggregation.Aggregates;
using YnabCli.Aggregation.Aggregator.ListAggregators;
using YnabCli.Aggregation.Extensions;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.RecurringTransactions;

public class RecurringTransactionsCommandHandler(ConfiguredBudgetClient budgetClient)
    : CommandHandler, ICommandHandler<RecurringTransactionsCommand>
{
    private const int DefaultMinimumOccurrences = 2;
    
    public async Task<CliCommandOutcome> Handle(RecurringTransactionsCommand command, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(command);

        var viewModel = new TransactionPayeeMemoOccurrenceCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }

    private async Task<ListYnabAggregator<TransactionPayeeMemoOccurrenceAggregate>> PrepareAggregator(RecurringTransactionsCommand command)
    {
        var budget =  await budgetClient.GetDefaultBudget();
        var transactions = await budget.GetTransactions();
        
        var aggregator = new TransactionPayeeMemoOccurrenceYnabAggregator(transactions);
            
        aggregator.BeforeAggregation(ts => ts.FilterOutCategories([YnabConstants.SplitCategoryId]));

        if (command.From.HasValue)
        {
            aggregator.BeforeAggregation(ts => ts.FilterFrom(command.From.Value));
        }
        
        if (command.To.HasValue)
        {
            aggregator.BeforeAggregation(ts => ts.FilterTo(command.To.Value));
        }

        if (command.PayeeName != null)
        {
            aggregator.BeforeAggregation(ts => ts.FilterToPayeeNames(command.PayeeName));
        }

        return aggregator
            .AfterAggregation(a => a.FilterToMinimumOccurrences(command.MinimumOccurrences ?? DefaultMinimumOccurrences))
            .AfterAggregation(a => a.OrderByDescending(aggregate => aggregate.MemoOccurrence));
    }
}
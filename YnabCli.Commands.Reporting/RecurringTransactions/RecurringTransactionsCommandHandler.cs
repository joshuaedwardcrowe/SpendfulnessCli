using ConsoleTables;
using Ynab;
using Ynab.Extensions;
using YnabCli.Aggregation.Aggregates;
using YnabCli.Aggregation.Aggregator.ListAggregators;
using YnabCli.Aggregation.Extensions;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.RecurringTransactions;

public class RecurringTransactionsCommandHandler : CommandHandler, ICommandHandler<RecurringTransactionsCommand>
{
    private readonly ConfiguredBudgetClient _budgetClient;
    private readonly TransactionMemoOccurrenceViewModelBuilder _viewModelBuilder;

    public RecurringTransactionsCommandHandler(
        ConfiguredBudgetClient budgetClient,
        TransactionMemoOccurrenceViewModelBuilder viewModelBuilder)
    {
        _budgetClient = budgetClient;
        _viewModelBuilder = viewModelBuilder;
    }

    private const int DefaultMinimumOccurrences = 2;
    
    public async Task<ConsoleTable> Handle(RecurringTransactionsCommand command, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(command);

        var viewModel = _viewModelBuilder
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }

    private async Task<ListAggregator<TransactionPayeeMemoOccurrenceAggregate>> PrepareAggregator(RecurringTransactionsCommand command)
    {
        var budget =  await _budgetClient.GetDefaultBudget();
        var transactions = await budget.GetTransactions();
        
        var aggregator = new TransactionPayeeMemoOccurrenceAggregator(transactions);
            
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
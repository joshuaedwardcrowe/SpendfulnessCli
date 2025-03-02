using ConsoleTables;
using Ynab;
using Ynab.Connected;
using Ynab.Extensions;
using YnabCli.Commands.Factories;
using YnabCli.Commands.Handlers;
using YnabCli.ViewModels.Aggregates;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.Extensions;
using YnabCli.ViewModels.ViewModelBuilders;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.Commands.Reporting.RecurringTransactions;

public class RecurringTransactionsCommandHandler : CommandHandler, ICommandHandler<RecurringTransactionsCommand>
{
    private readonly CommandBudgetGetter _budgetGetter;
    private readonly TransactionMemoOccurrenceViewModelBuilder _viewModelBuilder;

    public RecurringTransactionsCommandHandler(
        CommandBudgetGetter budgetGetter,
        TransactionMemoOccurrenceViewModelBuilder viewModelBuilder)
    {
        _budgetGetter = budgetGetter;
        _viewModelBuilder = viewModelBuilder;
    }

    private const int DefaultMinimumOccurrences = 2;
    
    public async Task<ConsoleTable> Handle(RecurringTransactionsCommand command, CancellationToken cancellationToken)
    {
        var budget =  await _budgetGetter.Get();
        
        var transactions = await GetTransactions(budget, command);

        var aggregator = new TransactionMemoOccurrenceAggregator(transactions)
            .AddAggregationOperation(a => FilterByMinimumOccurrences(command.MinimumOccurrences, a))
            .AddAggregationOperation(SortByMinimumOccurrence);

        var viewModel = _viewModelBuilder
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
    
    private async Task<IEnumerable<Transaction>> GetTransactions(ConnectedBudget connectedBudget, RecurringTransactionsCommand command)
    {
        var transactions = await connectedBudget.GetTransactions();

        var splitCategoryId = Guid.Parse("26330e86-4711-41f9-bd3e-a1c983da936a");

        var castedTransactions = transactions.FilterOutCategories([splitCategoryId]);

        if (command.From.HasValue)
        {
            castedTransactions = castedTransactions.FilterFrom(command.From.Value);
        }

        if (command.To.HasValue)
        {
            castedTransactions = castedTransactions.FilterTo(command.To.Value);
        }

        if (command.PayeeName != null)
        {
            castedTransactions = castedTransactions.FilterByPayeeName(command.PayeeName);
        }
        
        return castedTransactions;
    }

    private IEnumerable<TransactionMemoOccurrenceAggregate> FilterByMinimumOccurrences(int? minimumOccurrences,
        IEnumerable<TransactionMemoOccurrenceAggregate> aggregates)
            => aggregates.FilterByMinimumOccurrences(minimumOccurrences ?? DefaultMinimumOccurrences);
    
    private IEnumerable<TransactionMemoOccurrenceAggregate> SortByMinimumOccurrence(
        IEnumerable<TransactionMemoOccurrenceAggregate> aggregates)
            => aggregates.OrderByDescending(aggregate => aggregate.MemoOccurrence);
}
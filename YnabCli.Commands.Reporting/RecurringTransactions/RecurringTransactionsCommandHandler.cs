using ConsoleTables;
using Ynab;
using Ynab.Connected;
using Ynab.Extensions;
using YnabCli.Commands.Factories;
using YnabCli.Commands.Handlers;
using YnabCli.ViewModels.Aggregator;
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

        var aggregator = new TransactionMemoOccurrenceAggregator(transactions);

        _viewModelBuilder
            .AddAggregator(aggregator)
            .AddColumnNames(TransactionMemoOccurrenceViewModel.GetColumnNames());

        var viewModel = _viewModelBuilder
            .AddMinimumOccurrencesFilter(command.MinimumOccurrences ?? DefaultMinimumOccurrences)
            .AddSortOrder(ViewModelSortOrder.Descending)
            .Build();

        return Compile(viewModel);
    }
    
    private async Task<IEnumerable<Transaction>> GetTransactions(ConnectedBudget connectedBudget, RecurringTransactionsCommand command)
    {
        var transactions = await connectedBudget.GetTransactions();

        var castedTransactions = transactions.Cast<Transaction>();

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
}
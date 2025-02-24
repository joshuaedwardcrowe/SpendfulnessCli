using ConsoleTables;
using Ynab;
using Ynab.Clients;
using Ynab.Extensions;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.ViewModelBuilders;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.Commands.Reporting.RecurringTransactions;

public class RecurringTransactionsCommandHandler : CommandHandler, ICommandHandler<RecurringTransactionsCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly TransactionMemoOccurrenceViewModelBuilder _builder;
    private const int DefaultMinimumOccurrences = 2;

    public RecurringTransactionsCommandHandler(
        BudgetsClient budgetsClient,
        TransactionMemoOccurrenceViewModelBuilder builder)
    {
        _budgetsClient = budgetsClient;
        _builder = builder;
    }

    public async Task<ConsoleTable> Handle(RecurringTransactionsCommand command, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
        var budget =  budgets.First();
        
        var transactions = await GetTransactions(budget, command);

        var aggregator = new TransactionMemoOccurrenceAggregator(transactions);

        _builder
            .AddAggregator(aggregator)
            .AddColumnNames(TransactionMemoOccurrenceViewModel.GetColumnNames());

        var viewModel = _builder
            .AddMinimumOccurrencesFilter(command.MinimumOccurrences ?? DefaultMinimumOccurrences)
            .AddSortOrder(ViewModelSortOrder.Descending)
            .Build();

        return Compile(viewModel);
    }
    
    private async Task<IEnumerable<Transaction>> GetTransactions(Budget budget, RecurringTransactionsCommand command)
    {
        var transactions = await budget.GetTransactions();

        if (command.From.HasValue)
        {
            transactions = transactions.FilterFrom(command.From.Value);
        }

        if (command.To.HasValue)
        {
            transactions = transactions.FilterTo(command.To.Value);
        }

        if (command.PayeeName != null)
        {
            transactions = transactions.FilterByPayeeName(command.PayeeName);
        }
        
        return transactions;
    }
}
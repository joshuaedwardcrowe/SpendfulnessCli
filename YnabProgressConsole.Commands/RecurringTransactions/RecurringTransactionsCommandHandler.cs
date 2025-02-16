using ConsoleTables;
using Ynab.Clients;
using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.ViewModelBuilders;
using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Commands.RecurringTransactions;

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
        
        var transactions = await budget.GetTransactions();

        var evaluator = new TransactionMemoOccurrenceAggregator(transactions);

        _builder
            .AddAggregator(evaluator)
            .AddColumnNames(TransactionMemoOccurrenceViewModel.GetColumnNames());

        if (command.PayeeName != null)
        {
            _builder.AddPayeeNameFilter(command.PayeeName);
        }
        
        var minimumOccurrences = command.MinimumOccurrences ?? DefaultMinimumOccurrences;

        var viewModel = _builder
            .AddMinimumOccurrencesFilter(minimumOccurrences)
            .AddSortOrder(ViewModelSortOrder.Descending)
            .Build();

        return Compile(viewModel);
    }
}
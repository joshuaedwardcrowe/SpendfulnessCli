using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Clients;
using Ynab.Collections;
using Ynab.Extensions;
using YnabProgressConsole.Compilation;
using YnabProgressConsole.Compilation.TransactionsByMemoOccurrenceByPayeeNameView;

namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommandHandler
    : CommandHandler, ICommandHandler<RecurringTransactionsCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly IGroupViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName> _groupViewModelBuilder;
    private const int DefaultMinimumOccurrences = 2;

    public RecurringTransactionsCommandHandler(
        BudgetsClient budgetsClient,
        [FromKeyedServices(typeof(TransactionsByMemoOccurrenceByPayeeName))]
        IGroupViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName> groupViewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _groupViewModelBuilder = groupViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(RecurringTransactionsCommand command, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
        
        // TODO: Add support for selecting a budget if you ever do a settings feture.
        var budget =  budgets.First();
        
        var transactions = await budget.GetTransactions();

        var minimumOccurrences = command.MinimumOccurrences ?? DefaultMinimumOccurrences;

        var groups = transactions
            .FilterToSpending()
            .GroupByPayeeName(command.PayeeName)
            .GroupByMemoOccurence(minimumOccurrences);

        var viewModel = _groupViewModelBuilder
            .AddGroups(groups)
            .AddColumnNames(TransactionsByMemoOccurrenceByPayeeNameViewModel.GetColumnNames())
            .AddSortColumnName(TransactionsByMemoOccurrenceByPayeeNameViewModel.MemoOccurenceColumnName)
            .AddSortOrder(ViewModelSortOrder.Descending)
            .Build();
        
        return Compile(viewModel);
    }
}
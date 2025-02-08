using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Clients;
using Ynab.Collections;
using Ynab.Extensions;
using YnabProgressConsole.Compilation;
using YnabProgressConsole.Compilation.TransactionsByMemoOccurrenceByPayeeNameV;

namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommandHandler
    : CommandHandler, ICommandHandler<RecurringTransactionsCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly IGroupViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName> _groupViewModelBuilder;

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
        
        var groups = transactions
            .FilterToSpending()
            .GroupByPayeeName(command.PayeeName)
            .GroupByMemoOccurence(command.MinimumOccurrences);

        var viewModelColumnNames = TransactionsByMemoOccurrenceByPayeeNameViewModel.GetColumnNames();

        var viewModel = _groupViewModelBuilder
            .AddGroups(groups)
            .AddColumnNames(viewModelColumnNames.ToArray())
            .AddSortColumnName(TransactionsByMemoOccurrenceByPayeeNameViewModel.MemoOccurenceColumnName)
            .AddSortOrder(SortOrder.Descending)
            .Build();
        
        return Compile(viewModel);
    }
}
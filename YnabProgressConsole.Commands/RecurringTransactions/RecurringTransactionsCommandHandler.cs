using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Clients;
using Ynab.Collections;
using Ynab.Extensions;
using YnabProgressConsole.Compilation;
using YnabProgressConsole.Compilation.RecurringTransactions;

namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommandHandler
    : CommandHandler, ICommandHandler<RecurringTransactionsCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly IViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName> _viewModelBuilder;

    public RecurringTransactionsCommandHandler(
        BudgetsClient budgetsClient,
        [FromKeyedServices(typeof(TransactionsByMemoOccurrenceByPayeeName))]
        IViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName> viewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _viewModelBuilder = viewModelBuilder;
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

        var viewModel = _viewModelBuilder
            .AddGroups(groups)
            .AddColumnNames(viewModelColumnNames.ToArray())
            .AddSortColumnName(TransactionsByMemoOccurrenceByPayeeNameViewModel.MemoOccurenceColumnName)
            .AddSortOrder(SortOrder.Descending)
            .Build();
        
        return Compile(viewModel);
    }
}
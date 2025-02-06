using ConsoleTables;
using Ynab.Clients;
using Ynab.Collections;
using Ynab.Extensions;
using YnabProgressConsole.ViewModels;

namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommandHandler : CommandHandler, ICommandHandler<RecurringTransactionsCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly IViewModelConstructor<IEnumerable<TransactionsByMemoOccurrenceByPayeeName>> _viewModelConstructor;

    public RecurringTransactionsCommandHandler(
        BudgetsClient budgetsClient,
        IViewModelConstructor<IEnumerable<TransactionsByMemoOccurrenceByPayeeName>> viewModelConstructor)
    {
        _budgetsClient = budgetsClient;
        _viewModelConstructor = viewModelConstructor;
    }

    public async Task<ConsoleTable> Handle(RecurringTransactionsCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
        
        // TODO: Add support for selecting a budget if you ever do a settings feture.
        var budget =  budgets.First();
        
        var allTransactions = await budget.GetTransactions();

        // TODO: Could filter by account? payee?  greater/lower than values?
        var transactions = allTransactions
            .FilterToSpending()
            .GroupByPayeeName()
            .GroupByMemoOccurence();

        var viewModel = _viewModelConstructor.Construct(transactions);

        // TODO: This needs to render a proper view model.
        return Compile(viewModel);
    }
}
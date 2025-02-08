using ConsoleTables;
using Ynab.Clients;
using Ynab.Extensions;
using YnabProgressConsole.ViewModels;

namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommandHandler
    : CommandHandler, ICommandHandler<RecurringTransactionsCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly RecurringTransactionsViewModelConstructor _viewModelConstructor;

    public RecurringTransactionsCommandHandler(
        BudgetsClient budgetsClient,
        RecurringTransactionsViewModelConstructor viewModelConstructor)
    {
        _budgetsClient = budgetsClient;
        _viewModelConstructor = viewModelConstructor;
    }

    public async Task<ConsoleTable> Handle(RecurringTransactionsCommand command, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
        
        // TODO: Add support for selecting a budget if you ever do a settings feture.
        var budget =  budgets.First();
        
        var allTransactions = await budget.GetTransactions();
        
        var transactions = allTransactions
            .FilterToSpending()
            .GroupByPayeeName(command.PayeeName)
            .GroupByMemoOccurence(command.MinimumOccurrences);

        var viewModel = _viewModelConstructor.Construct(transactions);
        
        return Compile(viewModel);
    }
}
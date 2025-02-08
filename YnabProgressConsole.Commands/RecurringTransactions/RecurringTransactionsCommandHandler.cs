using ConsoleTables;
using Ynab.Clients;
using Ynab.Extensions;
using YnabProgressConsole.Compilation;

namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommandHandler
    : CommandHandler, ICommandHandler<RecurringTransactionsCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly RecurringTransactionsViewModelCompiler _viewModelCompiler;

    public RecurringTransactionsCommandHandler(
        BudgetsClient budgetsClient,
        RecurringTransactionsViewModelCompiler viewModelCompiler)
    {
        _budgetsClient = budgetsClient;
        _viewModelCompiler = viewModelCompiler;
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

        var viewModel = _viewModelCompiler.Compile(transactions);
        
        return Compile(viewModel);
    }
}
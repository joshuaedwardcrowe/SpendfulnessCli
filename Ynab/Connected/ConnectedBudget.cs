using Ynab.Clients;
using Ynab.Responses.Budgets;

namespace Ynab.Connected;

public class ConnectedBudget : Budget
{
    private readonly AccountsClient _accountsClient;
    private readonly CategoriesClient _categoriesClient;
    private readonly TransactionsClient _transactionsClient;
    private readonly ScheduledTransactionsClient _scheduledTransactionsClient;

    public ConnectedBudget(
        AccountsClient accountsClient,
        CategoriesClient categoriesClient,
        TransactionsClient transactionsClient,
        ScheduledTransactionsClient scheduledTransactionsClient,
        BudgetResponse budgetResponse) : base(budgetResponse)
    {
        _accountsClient = accountsClient;
        _categoriesClient = categoriesClient;
        _transactionsClient = transactionsClient;
        _scheduledTransactionsClient = scheduledTransactionsClient;
    }

    public Task<IEnumerable<Account>> GetAccounts()
        => _accountsClient.GetAccounts();

    public Task<IEnumerable<ConnectedCategoryGroup>> GetCategoryGroups()
        => _categoriesClient.GetCategoryGroups();
    
    public Task<IEnumerable<ConnectedTransaction>> GetTransactions()
        => _transactionsClient.GetTransactions();
    
    public Task<IEnumerable<ConnectedScheduledTransaction>> GetScheduledTransactions()
        => _scheduledTransactionsClient.GetScheduledTransactions();
}
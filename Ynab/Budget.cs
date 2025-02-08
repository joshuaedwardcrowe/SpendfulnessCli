using Ynab.Clients;
using Ynab.Extensions;
using Ynab.Responses.Budgets;

namespace Ynab;

public class Budget
{
    private readonly AccountsClient _accountsClient;
    private readonly CategoriesClient _categoriesClient;
    private readonly TransactionsClient _transactionsClient;
    private readonly ScheduledTransactionsClient _scheduledTransactionsClient;
    private readonly BudgetResponse _budget;


    public Budget(
        AccountsClient accountsClient,
        CategoriesClient categoriesClient,
        TransactionsClient transactionsClient,
        ScheduledTransactionsClient scheduledTransactionsClient,
        BudgetResponse budget)
    {
        _accountsClient = accountsClient;
        _categoriesClient = categoriesClient;
        _transactionsClient = transactionsClient;
        _scheduledTransactionsClient = scheduledTransactionsClient;
        _budget = budget;
    }
    
    public Task<IEnumerable<Account>> GetAccounts()
        => _accountsClient.GetAccounts();

    public Task<IEnumerable<CategoryGroup>> GetCategoryGroups()
        => _categoriesClient.GetCategoryGroups();
    
    public Task<IEnumerable<Transaction>> GetTransactions()
        => _transactionsClient.GetTransactions();
    
    public Task<IEnumerable<ScheduledTransaction>> GetScheduledTransactions()
        => _scheduledTransactionsClient.GetScheduledTransactions();
}
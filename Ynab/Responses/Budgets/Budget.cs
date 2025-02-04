using Ynab.Clients;
using Ynab.Extensions;

namespace Ynab.Responses.Budgets;

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

    public async Task<IEnumerable<Account>> GetCheckingAccounts()
    {
        var allAccounts = await _accountsClient.GetAccounts();
        return allAccounts.FilterToChecking();
    }

    public async Task<decimal> GetCheckingBalance()
    {
        var checkingAccounts = await GetCheckingAccounts();
        return checkingAccounts.Sum(checkingAccount => checkingAccount.Balance);
    }

    public Task<IEnumerable<CategoryGroup>> GetCategoryGroups()
        => _categoriesClient.GetCategoryGroups();
    
    public Task<IEnumerable<Transaction>> GetTransactions()
        => _transactionsClient.GetTransactions();
    
    public Task<IEnumerable<ScheduledTransaction>> GetScheduledTransactions()
        => _scheduledTransactionsClient.GetScheduledTransactions();
}
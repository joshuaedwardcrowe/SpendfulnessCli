using Ynab.Clients;
using Ynab.Responses.Budgets;

namespace Ynab.Connected;

public class ConnectedBudget : Budget
{
    private readonly AccountsClient _accountsClient;
    private readonly CategoriesClient _categoriesClient;
    private readonly TransactionsClient _transactionsClient;

    public ConnectedBudget(
        AccountsClient accountsClient,
        CategoriesClient categoriesClient,
        TransactionsClient transactionsClient,
        BudgetResponse budgetResponse) : base(budgetResponse)
    {
        _accountsClient = accountsClient;
        _categoriesClient = categoriesClient;
        _transactionsClient = transactionsClient;
    }

    public Task<IEnumerable<Account>> GetAccounts()
        => _accountsClient.GetAccounts();

    public Task<ConnectedAccount> GetAccount(Guid id)
        => _accountsClient.GetAccount(id);

    public Task<ConnectedAccount> CreateAccount(NewAccount newAccount) 
        => _accountsClient.CreateAccount(newAccount);
    
    public Task<IEnumerable<Transaction>> MoveTransactions(IEnumerable<MovedTransaction> transactions) 
        => _transactionsClient.MoveTransactions(transactions);

    public Task<IEnumerable<CategoryGroup>> GetCategoryGroups() => _categoriesClient.GetCategoryGroups();
    public Task<IEnumerable<Transaction>> GetTransactions() => _transactionsClient.GetTransactions();
    public Task<Transaction> GetTransaction(string id) => _transactionsClient.GetTransaction(id);
}
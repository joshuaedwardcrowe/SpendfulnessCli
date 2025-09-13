using Ynab.Clients;
using Ynab.Responses.Budgets;

namespace Ynab.Connected;

public class ConnectedBudget : Budget
{
    private readonly AccountsClient _accountsClient;
    private readonly CategoriesClient _categoriesClient;
    private readonly TransactionsClient _transactionsClient;
    private readonly ScheduledTransactionClient _scheduledTransactionsClient;

    public ConnectedBudget(
        AccountsClient accountsClient,
        CategoriesClient categoriesClient,
        TransactionsClient transactionsClient,
        ScheduledTransactionClient scheduledTransactionsClient,
        BudgetResponse budgetResponse) : base(budgetResponse)
    {
        _accountsClient = accountsClient;
        _categoriesClient = categoriesClient;
        _transactionsClient = transactionsClient;
        _scheduledTransactionsClient = scheduledTransactionsClient;
    }

    public Task<IEnumerable<Account>> GetAccounts() => _accountsClient.GetAccounts();
    public Task<ConnectedAccount> GetAccount(Guid id) => _accountsClient.GetAccount(id);
    public Task<IEnumerable<CategoryGroup>> GetCategoryGroups() => _categoriesClient.GetCategoryGroups();
    public Task<IEnumerable<Transaction>> GetTransactions() => _transactionsClient.GetTransactions();
    public Task<Transaction> GetTransaction(string id) => _transactionsClient.GetTransaction(id);
    public Task<ConnectedAccount> CreateAccount(NewAccount newAccount) => _accountsClient.CreateAccount(newAccount);
    
    public async Task MoveTransactions(ConnectedAccount originalAccount, ConnectedAccount newAccount)
    {
        var transactionsTask = originalAccount.GetTransactions();
        var scheduledTransactionsTask = originalAccount.GetScheduledTransactions();
        
        await Task.WhenAll(transactionsTask, scheduledTransactionsTask);

        // TODO: Move to mapper.
        var transactionsToMove = transactionsTask
            .Result
            .Where(transaction => transaction.PayeeName != AutomatedPayeeNames.StartingBalance)
            .Select(transaction => new MovedTransaction(transaction.Id, newAccount.Id))
            .ToList();
        
        var scheduledTransactionsToMove = scheduledTransactionsTask
            .Result
            .Select(scheduledTransaction => new MovedScheduledTransaction(scheduledTransaction.Id, newAccount.Id))
            .ToList();
        
        var moveTransactionsTask  = _transactionsClient.MoveTransactions(transactionsToMove);
        var moveScheduledTransactionsTask =  _scheduledTransactionsClient.MoveTransaction(scheduledTransactionsToMove);
        
        await Task.WhenAll(moveTransactionsTask, moveScheduledTransactionsTask);
    }
}
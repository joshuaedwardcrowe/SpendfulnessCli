using Ynab.Clients;

namespace Ynab.Responses.Budgets;

public class Budget
{
    private readonly CategoriesClient _categoriesBaseClient;
    private readonly TransactionsClient _transactionsBaseClient;
    private readonly BudgetResponse _budget;

    public Budget(CategoriesClient categoriesBaseClient, TransactionsClient transactionsClient, BudgetResponse budget)
    {
        _categoriesBaseClient = categoriesBaseClient;
        _transactionsBaseClient = transactionsClient;
        _budget = budget;
    }

    public Task<IEnumerable<CategoryGroup>> GetCategoryGroups()
        => _categoriesBaseClient.GetCategoryGroups();
    
    public Task<IEnumerable<Transaction>> GetTransactions()
        => _transactionsBaseClient.GetTransactions();
}
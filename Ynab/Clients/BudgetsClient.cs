using Ynab.Responses.Budgets;

namespace Ynab.Clients;

public class BudgetsClient : YnabApiClient
{
    private const string BudgetsApiPath = "budgets";
    private readonly YnabHttpClientFactory _ynabHttpClientFactory;

    public BudgetsClient(YnabHttpClientFactory ynabHttpClientFactory)
    {
        _ynabHttpClientFactory = ynabHttpClientFactory;
    }
    
    public async Task<IEnumerable<Budget>> GetBudgets()
    {
        var response = await Get<GetBudgetsResponseData>(BudgetsApiPath);
        return ConvertBudgetResponsesToWrappers(response.Data.Budgets);
    }

    private IEnumerable<Budget> ConvertBudgetResponsesToWrappers(IEnumerable<BudgetResponse> budgetResponses)
    {
        foreach (var budgetResponse in budgetResponses)
        {
            var parentApiPath = $"{BudgetsApiPath}/{budgetResponse.Id}";
            
            var accounts = new AccountsClient(_ynabHttpClientFactory, parentApiPath);
            var categories = new CategoriesClient(_ynabHttpClientFactory, parentApiPath);
            var transactions = new TransactionsClient(_ynabHttpClientFactory, parentApiPath);
            var scheduledTransactions = new ScheduledTransactionsClient(_ynabHttpClientFactory, parentApiPath);

            yield return new Budget(
                accounts,
                categories,
                transactions,
                scheduledTransactions,
                budgetResponse);
        }
    }

    protected override HttpClient GetHttpClient() => _ynabHttpClientFactory.Create();
}
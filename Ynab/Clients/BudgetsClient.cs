using Ynab.Responses.Budgets;

namespace Ynab.Clients;

public class BudgetsClient() : YnabApiClient([])
{
    private const string BudgetsApiPathSuffix = "budgets";

    public async Task<IEnumerable<Budget>> GetBudgets()
    {
        var response = await Get<GetBudgetsResponseData>(BudgetsApiPathSuffix);
        return ConvertBudgetResponsesToWrappers(response.Data.Budgets);
    }

    private IEnumerable<Budget> ConvertBudgetResponsesToWrappers(IEnumerable<BudgetResponse> budgets)
    {
        foreach (var budget in budgets)
        {
            var parentApiPath = $"{HttpClient.BaseAddress}/{BudgetsApiPathSuffix}/{budget.Id}/";

            var accountsApiClient = new AccountsClient(parentApiPath, RequestLogs);
            var categoriesApiClient = new CategoriesClient(parentApiPath, RequestLogs);
            var transactionsApiClient = new TransactionsClient(parentApiPath, RequestLogs);
            var scheduledTransactionsApiClient = new ScheduledTransactionsClient(parentApiPath, RequestLogs);
            
            yield return new Budget(
                accountsApiClient,
                categoriesApiClient,
                transactionsApiClient,
                scheduledTransactionsApiClient,
                budget);
        }
    }
}
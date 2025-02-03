using Ynab.Responses;
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

            var categoryApiClient = new CategoriesClient(parentApiPath, RequestLogs);
            var transactionsApiClient = new TransactionsClient(parentApiPath, RequestLogs);
            
            yield return new Budget(categoryApiClient, transactionsApiClient, budget);
        }
    }
}
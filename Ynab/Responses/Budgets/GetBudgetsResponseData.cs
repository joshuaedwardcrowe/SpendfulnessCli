namespace Ynab.Responses.Budgets;

public class GetBudgetsResponseData
{
    public required IEnumerable<BudgetResponse> Budgets { get; set; }
}
using Ynab.Responses.Budgets;

namespace Ynab;

public class Budget(BudgetResponse budgetResponse)
{
    public Guid Id => budgetResponse.Id;
}
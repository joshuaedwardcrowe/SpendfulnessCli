using Ynab.Responses.Budgets;

namespace Ynab;

public class Budget(BudgetResponse budgetResponse)
{
    /// <summary>
    /// Unique identifier of the budget.
    /// </summary>
    public Guid Id => budgetResponse.Id;
    
    /// <summary>
    /// When the budget was created.
    /// </summary>
    public DateOnly Created => budgetResponse.FirstMonth;
    
    /// <summary>
    /// When the budget was last active.
    /// </summary>
    public DateOnly LastActive => budgetResponse.LastMonth;
}
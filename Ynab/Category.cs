using Ynab.Responses.Category;
using Ynab.Sanitisers;

namespace Ynab;

public class Category(CategoryResponse categoryResponse)
{
    public Guid Id => categoryResponse.Id;
    public string Name => categoryResponse.Name;
    public int GoalTarget => categoryResponse.GoalTarget;
    public DateOnly? GoalCreationMonth => categoryResponse.GoalCreationMonth;
    public DateOnly? GoalTargetMonth => categoryResponse.GoalTargetMonth;
    public decimal? GoalOverallFunded => MilliunitSanitiser.Calculate(categoryResponse.GoalOverallFunded);
    public decimal? GoalOverallLeft => MilliunitSanitiser.Calculate(categoryResponse.GoalOverallLeft);
    
    public bool HasGoal => GoalTarget > 0;
}
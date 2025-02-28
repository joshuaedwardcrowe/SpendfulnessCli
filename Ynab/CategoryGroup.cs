using Ynab.Responses.Categories;
using Ynab.Sanitisers;

namespace Ynab;

public class CategoryGroup(CategoryGroupResponse categoryGroupResponse)
{
    public string Name => categoryGroupResponse.Name;
    
    /// <summary>
    /// Money in these categories available to spend.
    /// </summary>
    public decimal Available
        => categoryGroupResponse.Categories.Sum(category => MilliunitSanitiser.Calculate(category.Available));
    
    public IEnumerable<Guid> GetCategoryIds()
        => categoryGroupResponse.Categories.Select(category => category.Id);
}
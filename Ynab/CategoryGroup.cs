using Ynab.Clients;
using Ynab.Responses.Categories;
using Ynab.Sanitisers;

namespace Ynab;

public class CategoryGroup
{
    private readonly CategoriesClient _categoryClient;
    private readonly CategoryGroupResponse _categoryGroupResponse;

    public string Name => _categoryGroupResponse.Name;

    public CategoryGroup(CategoriesClient categoryClient, CategoryGroupResponse categoryGroupResponse)
    {
        _categoryClient = categoryClient;
        _categoryGroupResponse = categoryGroupResponse;
    }
    
    public IEnumerable<Category> Categories => _categoryGroupResponse
        .Categories
        .Where(category => !YnabConstants.AutomatedCategoryNames.Contains(category.Name))
        .Select(category => new Category(_categoryClient, category));
    
    /// <summary>
    /// Money in these categories available to spend.
    /// </summary>
    public decimal Available => _categoryGroupResponse.Categories.Sum(category =>
        MilliunitSanitiser.Calculate(category.Available));
    
    public IEnumerable<Guid> GetCategoryIds() => Categories
        .Where(category => category.Id.HasValue)
        .Select(category => category.Id.Value);
}
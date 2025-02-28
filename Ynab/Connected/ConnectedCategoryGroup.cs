using Ynab.Clients;
using Ynab.Responses.Categories;

namespace Ynab.Connected;

public class ConnectedCategoryGroup : CategoryGroup
{
    private readonly CategoriesClient _categoryClient;
    private readonly CategoryGroupResponse _categoryGroupResponse;

    public ConnectedCategoryGroup(CategoriesClient categoryClient, CategoryGroupResponse categoryGroupResponse) 
        : base(categoryGroupResponse)
    {
        _categoryClient = categoryClient;
        _categoryGroupResponse = categoryGroupResponse;
    }
    
    public IEnumerable<ConnectedCategory> Categories => _categoryGroupResponse
        .Categories
        .Where(category => !YnabConstants.AutomatedCategoryNames.Contains(category.Name))
        .Select(category => new ConnectedCategory(_categoryClient, category));
}
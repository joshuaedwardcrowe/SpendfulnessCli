using Ynab.Clients;
using Ynab.Responses.Categories;

namespace Ynab;

public class CategoryGroup
{
    private readonly CategoriesClient _categoryClient;
    private readonly CategoryGroupResponse _categoryGroupResponse;

    public Guid Id => _categoryGroupResponse.Id;
    public string Name => _categoryGroupResponse.Name;

    public IEnumerable<Category> Categories => _categoryGroupResponse.Categories
        .Select(categories => new Category(_categoryClient, categories));

    public CategoryGroup(CategoriesClient categoryClient, CategoryGroupResponse categoryGroupResponse)
    {
        _categoryClient = categoryClient;
        _categoryGroupResponse = categoryGroupResponse;
    }
}
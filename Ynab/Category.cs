using Ynab.Clients;
using Ynab.Responses.Category;

namespace Ynab;

public class Category
{
    private readonly CategoriesClient _categoriesClient;
    private CategoryResponse _categoryResponse;

    public Guid Id => _categoryResponse.Id;

    public Category(CategoriesClient categoriesClient, CategoryResponse categoryResponse)
    {
        _categoriesClient = categoriesClient;
        _categoryResponse = categoryResponse;
    }
}
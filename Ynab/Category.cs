using Ynab.Clients;
using Ynab.Responses.Category;

namespace Ynab;

public class Category(CategoriesClient categoriesClient, CategoryResponse categoryResponse)
{
    private readonly CategoriesClient _categoriesClient = categoriesClient;

    public Guid? Id => categoryResponse?.Id;
    public string Name => categoryResponse.Name;
}
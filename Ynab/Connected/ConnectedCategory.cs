using Ynab.Clients;
using Ynab.Responses.Category;

namespace Ynab.Connected;

public class ConnectedCategory(CategoriesClient categoriesClient, CategoryResponse categoryResponse)
    : Category(categoryResponse)
{
    private readonly CategoriesClient _categoriesClient = categoriesClient;
}
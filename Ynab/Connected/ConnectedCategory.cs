using Ynab.Clients;
using Ynab.Responses.Category;

namespace Ynab.Connected;

#pragma warning disable CS9113 // Parameter is unread.
public class ConnectedCategory(CategoriesClient categoriesClient, CategoryResponse categoryResponse)
#pragma warning restore CS9113 // Parameter is unread.
    : Category(categoryResponse)
{

}
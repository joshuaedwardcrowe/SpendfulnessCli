using Ynab.Responses;
using Ynab.Responses.Categories;

namespace Ynab.Clients;

public class CategoriesClient : YnabApiClient
{
    private const string CategoriesApiPath = "categories";
    public CategoriesClient(string parentApiPath, List<ApiRequestLog> requestLog) : base(requestLog)
    {
        HttpClient.BaseAddress = new Uri(parentApiPath);
    }

    public async Task<IEnumerable<CategoryGroup>> GetCategoryGroups()
    {
        var response = await Get<GetCategoriesResponseData>(CategoriesApiPath);
        return response.Data.CategoryGroups.Select(cg => new CategoryGroup(this, cg));
    }
}
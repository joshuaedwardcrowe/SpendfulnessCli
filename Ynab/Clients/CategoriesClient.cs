using Ynab.Responses.Categories;

namespace Ynab.Clients;

public class CategoriesClient : YnabApiClient
{
    private const string CategoriesApiPath = "categories";
    private readonly YnabHttpClientFactory _ynabHttpClientFactory;
    private readonly string _parentApiPath;

    public CategoriesClient(YnabHttpClientFactory ynabHttpClientFactory, string parentApiPath)
    {
        _ynabHttpClientFactory = ynabHttpClientFactory;
        _parentApiPath = parentApiPath;
    }
    
    public async Task<IEnumerable<CategoryGroup>> GetCategoryGroups()
    {
        var response = await Get<GetCategoriesResponseData>(CategoriesApiPath);
        return response.Data.CategoryGroups.Select(cg => new CategoryGroup(this, cg));
    }
    
    protected override HttpClient GetHttpClient() =>
        _ynabHttpClientFactory.Create(_parentApiPath, CategoriesApiPath);
}
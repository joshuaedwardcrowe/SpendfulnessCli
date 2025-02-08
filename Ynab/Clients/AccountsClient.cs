using Ynab.Http;
using Ynab.Responses.Accounts;

namespace Ynab.Clients;

public class AccountsClient : YnabApiClient
{
    private const string AccountsApiPath = "accounts";
    private readonly YnabHttpClientFactory _ynabHttpClientFactory;
    private readonly string _parentApiPath;

    public AccountsClient(YnabHttpClientFactory ynabHttpClientFactory, string parentApiPath)
    {
        _ynabHttpClientFactory = ynabHttpClientFactory;
        _parentApiPath = parentApiPath;
    }
    
    public async Task<IEnumerable<Account>> GetAccounts()
    {
        var response = await Get<GetAccountsResponseData>(string.Empty);
        return response.Data.Accounts.Select(a => new Account(this, a));
    }
    
    protected override HttpClient GetHttpClient() => 
        _ynabHttpClientFactory.Create(_parentApiPath, AccountsApiPath);
}
using Ynab.Connected;
using Ynab.Http;
using Ynab.Responses.Accounts;

namespace Ynab.Clients;

public class AccountsClient(YnabHttpClientBuilder ynabHttpClientBuilder, string parentApiPath)
    : YnabApiClient
{
    private const string AccountsApiPath = "accounts";

    public async Task<IEnumerable<Account>> GetAccounts()
    {
        var response = await Get<GetAccountsResponseData>(string.Empty);
        return response.Data.Accounts.Select(a => new ConnectedAccount(this, a));
    }
    
    protected override HttpClient GetHttpClient() => 
        ynabHttpClientBuilder.Build(parentApiPath, AccountsApiPath);
}
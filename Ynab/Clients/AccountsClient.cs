using Ynab.Connected;
using Ynab.Http;
using Ynab.Responses.Accounts;
using Ynab.Sanitisers;

namespace Ynab.Clients;

public class AccountsClient(YnabHttpClientBuilder builder, string parentApiPath) : YnabApiClient
{
    private const string AccountsApiPath = "accounts";

    public async Task<IEnumerable<Account>> GetAccounts()
    {
        var response = await Get<GetAccountsResponseData>(string.Empty);
        return response.Data.Accounts.Select(a => new Account(a));
    }
    
    public async Task<ConnectedAccount> GetAccount(Guid id)
    {
        var response = await Get<GetAccountResponseData>($"{AccountsApiPath}/{id}");
        return ConvertAccountResponseToConnectedAccount(response.Data.Account);
    }
    
    public async Task<ConnectedAccount> CreateAccount(NewAccount newAccount)
    {
        // TODO: Should I use some kind of mapper?
        var request = new CreateAccountRequest
        {
            Account = new AccountRequest
            {
                Name = newAccount.Name,
                Type = newAccount.Type,
                Balance = newAccount.Balance
            }
        };

        var response = await Post<CreateAccountRequest, CreateAccountResponse>(AccountsApiPath, request);
        return ConvertAccountResponseToConnectedAccount(response.Data.Account);
    }
    
    private ConnectedAccount ConvertAccountResponseToConnectedAccount(AccountResponse accountResponse)
    {
        var transactionClient = new TransactionsClient(builder, parentApiPath);
        var scheduledTransactionClient = new ScheduledTransactionClient(builder, parentApiPath);
        return new ConnectedAccount(transactionClient, scheduledTransactionClient, accountResponse);
    }
    
    protected override HttpClient GetHttpClient() => builder.Build(parentApiPath, AccountsApiPath);
}
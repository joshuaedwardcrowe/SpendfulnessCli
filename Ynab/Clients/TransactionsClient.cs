using Ynab.Connected;
using Ynab.Http;
using Ynab.Responses.Transactions;

namespace Ynab.Clients;

public class TransactionsClient(YnabHttpClientBuilder ynabHttpClientBuilder, string parentApiPath) : YnabApiClient
{
    private const string TransactionsApiPath = "transactions";

    public async Task<IEnumerable<ConnectedTransaction>> GetTransactions()
    {
        var response = await Get<GetTransactionsResponseData>(TransactionsApiPath);
        return response.Data.Transactions.Select(t => new ConnectedTransaction(this, t));
    }
    
    protected override HttpClient GetHttpClient()
        => ynabHttpClientBuilder.Build(parentApiPath,  TransactionsApiPath);
}


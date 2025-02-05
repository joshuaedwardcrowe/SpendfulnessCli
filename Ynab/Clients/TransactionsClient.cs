using Ynab.Responses.Transactions;

namespace Ynab.Clients;

public class TransactionsClient : YnabApiClient
{
    private const string TransactionsApiPath = "transactions";
    private readonly YnabHttpClientFactory _ynabHttpClientFactory;
    private readonly string _parentApiPath;

    public TransactionsClient(YnabHttpClientFactory ynabHttpClientFactory, string parentApiPath)
    {
        _ynabHttpClientFactory = ynabHttpClientFactory;
        _parentApiPath = parentApiPath;
    }
    
    public async Task<IEnumerable<Transaction>> GetTransactions()
    {
        var response = await Get<GetTransactionsResponseData>(TransactionsApiPath);
        return response.Data.Transactions.Select(t => new Transaction(t));
    }
    
    protected override HttpClient GetHttpClient()
        => _ynabHttpClientFactory.Create(_parentApiPath,  TransactionsApiPath);
}


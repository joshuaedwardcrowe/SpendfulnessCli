using Ynab.Responses;
using Ynab.Responses.Transactions;

namespace Ynab.Clients;

public class TransactionsClient : YnabApiClient
{
    private const string TransactionsApiPathSuffix = "transactions";

    public TransactionsClient(string parentApiPath, List<ApiRequestLog> logs) : base(logs)
    {
        HttpClient.BaseAddress = new Uri(parentApiPath);
    }
    
    public async Task<IEnumerable<Transaction>> GetTransactions()
    {
        var response = await Get<GetTransactionsResponseData>(TransactionsApiPathSuffix);
        return response.Data.Transactions.Select(t => new Transaction(t));
    }
}
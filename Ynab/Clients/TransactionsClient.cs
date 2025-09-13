using Ynab.Http;
using Ynab.Responses.Transactions;

namespace Ynab.Clients;

public class TransactionsClient(YnabHttpClientBuilder builder, string parentApiPath) : YnabApiClient
{
    private const string TransactionsApiPath = "transactions";

    public async Task<IEnumerable<Transaction>> GetTransactions()
    {
        var response = await Get<GetTransactionsResponse>(TransactionsApiPath);
        return response.Data.Transactions.Select(t => new Transaction(t));
    }

    public async Task<Transaction> GetTransaction(string id)
    {
        var response = await Get<GetTransactionResponse>($"{TransactionsApiPath}/{id}");
        return new Transaction(response.Data.Transaction);
    }
    
    protected override HttpClient GetHttpClient() => builder.Build(parentApiPath,  TransactionsApiPath);
}


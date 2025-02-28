using Ynab.Connected;
using Ynab.Http;
using Ynab.Responses.ScheduledTransactions;

namespace Ynab.Clients;

public class ScheduledTransactionsClient(YnabHttpClientBuilder ynabHttpClientBuilder, string parentApiPath)
    : YnabApiClient
{
    private const string ScheduledTransactionsPath = "scheduled_transactions";

    public async Task<IEnumerable<ConnectedScheduledTransaction>> GetScheduledTransactions()
    {
        var response = await Get<GetScheduledTransactionResponseData>(ScheduledTransactionsPath);
        return response.Data.ScheduledTransactions.Select(st => new ConnectedScheduledTransaction(this, st));
    }
    
    protected override HttpClient GetHttpClient()
        => ynabHttpClientBuilder.Build(parentApiPath, ScheduledTransactionsPath);
}
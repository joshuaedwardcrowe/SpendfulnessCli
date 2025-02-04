using Ynab.Responses.ScheduledTransactions;

namespace Ynab.Clients;

public class ScheduledTransactionsClient : YnabApiClient
{
    private const string TransactionsApiPathSuffix = "scheduled_transactions";
    
    public ScheduledTransactionsClient(string parentApiPath, List<ApiRequestLog> requestLog) : base(requestLog)
    {
        HttpClient.BaseAddress = new Uri(parentApiPath);
    }

    public async Task<IEnumerable<ScheduledTransaction>> GetScheduledTransactions()
    {
        var response = await Get<GetScheduledTransactionResponseData>(TransactionsApiPathSuffix);
        return response.Data.ScheduledTransactions.Select(st => new ScheduledTransaction(this, st));
    }
}
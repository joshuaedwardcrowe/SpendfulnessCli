using Ynab.Responses.ScheduledTransactions;

namespace Ynab.Clients;

public class ScheduledTransactionsClient : YnabApiClient
{
    private const string ScheduledTransactionsPath = "scheduled_transactions";
    private readonly YnabHttpClientFactory _ynabHttpClientFactory;
    private readonly string _parentApiPath;

    public ScheduledTransactionsClient(YnabHttpClientFactory ynabHttpClientFactory, string parentApiPath)
    {
        _ynabHttpClientFactory = ynabHttpClientFactory;
        _parentApiPath = parentApiPath;
    }
    
    public async Task<IEnumerable<ScheduledTransaction>> GetScheduledTransactions()
    {
        var response = await Get<GetScheduledTransactionResponseData>(ScheduledTransactionsPath);
        return response.Data.ScheduledTransactions.Select(st => new ScheduledTransaction(this, st));
    }
    
    protected override HttpClient GetHttpClient()
        => _ynabHttpClientFactory.Create(_parentApiPath, ScheduledTransactionsPath);
}
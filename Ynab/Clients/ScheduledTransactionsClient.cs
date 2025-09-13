using Ynab.Http;
using Ynab.Requests.ScheduledTransactions;
using Ynab.Responses.ScheduledTransactions;

namespace Ynab.Clients;

public class ScheduledTransactionClient(YnabHttpClientBuilder builder, string parentApiPath) : YnabApiClient
{
    // TODO: Might move to this to a constants file.
    private string ScheduledTransactionsApiPath => "scheduled_transactions";
    
    // TODO: I'd prefer a less verbose naming style.
    public async Task<IEnumerable<ScheduledTransaction>> GetScheduledTransactions()
    {
        var response = await Get<GetScheduledTransactionResponseData>(ScheduledTransactionsApiPath);
        return response.Data.ScheduledTransactions.Select(st => new ScheduledTransaction(st));
    }

    public async Task<IEnumerable<ScheduledTransaction>> MoveTransaction(
        IEnumerable<MovedScheduledTransaction> movedTransactions)
    {
        // TODO: Some kind of mapper.
        var requests = movedTransactions
            .Select(transaction => new ScheduledTransactionRequest
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId
            });

        var request = new UpdateScheduledTransactionRequest
        {
            ScheduledTransactions = requests
        };
         
        var response = await Patch<UpdateScheduledTransactionRequest, GetScheduledTransactionResponseData>(ScheduledTransactionsApiPath, request);
        return response.Data.ScheduledTransactions.Select(transaction => new ScheduledTransaction(transaction));
    }
    
    protected override HttpClient GetHttpClient() => builder.Build(parentApiPath,  ScheduledTransactionsApiPath);
}

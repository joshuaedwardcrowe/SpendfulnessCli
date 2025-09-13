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
        var response = await Get<GetScheduledTransactionsResponseData>(ScheduledTransactionsApiPath);
        return response.Data.ScheduledTransactions.Select(st => new ScheduledTransaction(st));
    }

    public async Task<ScheduledTransaction> MoveTransaction(MovedScheduledTransaction movedScheduledTransaction)
    {
        var requestUrl = $"{ScheduledTransactionsApiPath}/{movedScheduledTransaction.Id}";
        
        // TODO: Some kind of mapper.
        var request = new UpdateScheduledTransactionRequest
        {
            ScheduledTransaction = new ScheduledTransactionRequest
            {
                Id = movedScheduledTransaction.Id,
                AccountId = movedScheduledTransaction.AccountId
            }
        };
         
        var response = await Put<UpdateScheduledTransactionRequest, GetScheduledTransactionResponseData>(requestUrl, request);
        return new ScheduledTransaction(response.Data.ScheduledTransaction);
    }
    
    protected override HttpClient GetHttpClient() => builder.Build(parentApiPath,  ScheduledTransactionsApiPath);
}

using Ynab.Http;
using Ynab.Requests.Transactions;
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

    public async Task<IEnumerable<Transaction>> MoveTransactions(IEnumerable<MovedTransaction> movedTransactions)
    {
        // TODO: some kind of mapper.
        var requests = movedTransactions
            .Select(transaction => new TransactionRequest
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId
            });

        var request = new UpdateTransactionRequest
        {
            Transactions = requests
        };
        
        var response = await Patch<UpdateTransactionRequest, GetTransactionsResponse>(TransactionsApiPath, request);
        return response.Data.Transactions.Select(transaction => new Transaction(transaction));
    }
    
    protected override HttpClient GetHttpClient() => builder.Build(parentApiPath,  TransactionsApiPath);
}


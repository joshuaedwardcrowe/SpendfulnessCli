using Ynab;
using Ynab.Factories;
using Ynab.Responses.Transactions;

namespace SpendfulnessCli.Abstractions.Splittables;

public class SplittableTransactionFactory : ITransactionFactory
{
    public bool CanWorkWith(TransactionResponse transactionResponse)
    {
        if (string.IsNullOrWhiteSpace(transactionResponse.Memo))
        {
            return false;
        }
        
        var hasMultipleMemoNotes = transactionResponse.Memo.Contains(',');
        if (!hasMultipleMemoNotes)
        {
            return false;
        }

        var memoNotes = transactionResponse.Memo.Split(",");

        return memoNotes.Length > 1;
    }

    public Transaction Create(TransactionResponse transactionResponse) 
        => new SplittableTransaction(transactionResponse);
}
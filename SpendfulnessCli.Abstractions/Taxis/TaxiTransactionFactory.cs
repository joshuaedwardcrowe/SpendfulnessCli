using System.Text.RegularExpressions;
using YnabSharp;
using YnabSharp.Factories;
using YnabSharp.Responses.Transactions;

namespace SpendfulnessCli.Abstractions.Taxis;

// TODO: Write unit tests.
public class TaxiTransactionFactory : ITransactionFactory
{
    private Regex _taxiRegex = new Regex(@"(?i)\b(.+?)\s+to\s+(.+?)\b", RegexOptions.IgnoreCase);

    public bool CanWorkWith(TransactionResponse transactionResponse)
        => transactionResponse.Memo != null && _taxiRegex.IsMatch(transactionResponse.Memo);

    public Transaction Create(TransactionResponse transactionResponse)
        => new TaxiTransaction(transactionResponse);
}
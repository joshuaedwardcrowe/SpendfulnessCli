using System.Text.RegularExpressions;
using Ynab;
using Ynab.Factories;
using Ynab.Responses.Transactions;

namespace Spendfulness.Cli.Abstractions.Taxis;

// TODO: Write unit tests.
public class TaxiTransactionFactory : ITransactionFactory
{
    private Regex _taxiRegex = new Regex(@"(?i)\b(.+?)\s+to\s+(.+?)\b", RegexOptions.IgnoreCase);

    public bool CanWorkWith(TransactionResponse transactionResponse)
        => transactionResponse.Memo != null && _taxiRegex.IsMatch(transactionResponse.Memo);

    public Transaction Create(TransactionResponse transactionResponse)
        => new TaxiTransaction(transactionResponse);
}
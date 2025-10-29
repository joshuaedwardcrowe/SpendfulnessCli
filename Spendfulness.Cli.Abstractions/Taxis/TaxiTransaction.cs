using Ynab;
using Ynab.Responses.Transactions;

namespace Spendfulness.Cli.Abstractions.Taxis;

public class TaxiTransaction(TransactionResponse transactionResponse) : Transaction(transactionResponse)
{
    // TODO: But specific behaviour here.
}
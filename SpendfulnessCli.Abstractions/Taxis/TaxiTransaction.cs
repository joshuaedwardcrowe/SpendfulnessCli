using Ynab;
using Ynab.Responses.Transactions;

namespace SpendfulnessCli.Abstractions.Taxis;

public class TaxiTransaction(TransactionResponse transactionResponse) : Transaction(transactionResponse)
{
    // TODO: But specific behaviour here.
}
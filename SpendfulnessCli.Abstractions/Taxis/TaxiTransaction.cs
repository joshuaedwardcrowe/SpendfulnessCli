using YnabSharp;
using YnabSharp.Responses.Transactions;

namespace SpendfulnessCli.Abstractions.Taxis;

public class TaxiTransaction(TransactionResponse transactionResponse) : Transaction(transactionResponse)
{
}
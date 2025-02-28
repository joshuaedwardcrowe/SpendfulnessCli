using Ynab.Clients;
using Ynab.Responses.Transactions;

namespace Ynab.Connected;

public class ConnectedTransaction( TransactionsClient transactionsClient, TransactionResponse transactionResponse)
    : Transaction(transactionResponse)
{
}
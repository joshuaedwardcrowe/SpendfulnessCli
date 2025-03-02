using Ynab.Clients;
using Ynab.Responses.Transactions;

namespace Ynab.Connected;

#pragma warning disable CS9113 // Parameter is unread.
public class ConnectedTransaction( TransactionsClient transactionsClient, TransactionResponse transactionResponse)
#pragma warning restore CS9113 // Parameter is unread.
    : Transaction(transactionResponse)
{
}
using Ynab.Clients;
using Ynab.Responses.ScheduledTransactions;

namespace Ynab.Connected;

public class ConnectedScheduledTransaction(
#pragma warning disable CS9113 // Parameter is unread.
    ScheduledTransactionsClient scheduledTransactionsClient,
#pragma warning restore CS9113 // Parameter is unread.
    ScheduledTransactionsResponse scheduledTransactionsResponse)
    : ScheduledTransaction(scheduledTransactionsResponse)
{
}
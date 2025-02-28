using Ynab.Clients;
using Ynab.Responses.ScheduledTransactions;

namespace Ynab.Connected;

public class ConnectedScheduledTransaction(
    ScheduledTransactionsClient scheduledTransactionsClient,
    ScheduledTransactionsResponse scheduledTransactionsResponse)
    : ScheduledTransaction(scheduledTransactionsResponse)
{
    private readonly ScheduledTransactionsClient _scheduledTransactionsClient;
}
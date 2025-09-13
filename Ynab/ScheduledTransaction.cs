using Ynab.Responses.ScheduledTransactions;
using Ynab.Sanitisers;

namespace Ynab;

public class ScheduledTransaction(ScheduledTransactionsResponse scheduledTransactionsResponse)
{
    public decimal Amount => MilliunitSanitiser.Calculate(scheduledTransactionsResponse.Amount);
    public DateTime NextOccurence => scheduledTransactionsResponse.NextOccurence;
    public Guid AccountId => scheduledTransactionsResponse.AccountId;
}
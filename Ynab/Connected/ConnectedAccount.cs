using Ynab.Clients;
using Ynab.Responses.Accounts;

namespace Ynab.Connected;

public class ConnectedAccount : Account
{
    private readonly TransactionsClient _transactionsClient;
    private readonly ScheduledTransactionClient _scheduledTransactionsClient;

    public ConnectedAccount(
        TransactionsClient transactionsClient,
        ScheduledTransactionClient scheduledTransactionsClient,
        AccountResponse accountResponse) : base(accountResponse)
    {
        _transactionsClient = transactionsClient;
        _scheduledTransactionsClient = scheduledTransactionsClient;
    }


    public async Task<IEnumerable<Transaction>> GetTransactions()
    {
        var transactions = await _transactionsClient.GetTransactions();
        return transactions.Where(t => t.AccountId == Id);
    }

    public async Task<IEnumerable<ScheduledTransaction>> GetScheduledTransactions()
    {
        var scheduledTransactions = await _scheduledTransactionsClient.GetScheduledTransactions();
        return scheduledTransactions.Where(st => st.AccountId == Id);
    }
}


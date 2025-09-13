using Ynab.Clients;
using Ynab.Responses.Accounts;

namespace Ynab.Connected;

public class ConnectedAccount : Account
{
    private readonly TransactionsClient _transactionsClient;

    public ConnectedAccount(TransactionsClient transactionsClient, AccountResponse accountResponse)
        : base(accountResponse)
    {
        _transactionsClient = transactionsClient;
    }

    public async Task<IEnumerable<Transaction>> GetTransactions()
    {
        var transactions = await _transactionsClient.GetTransactions();
        return transactions.Where(t => t.AccountId == Id);
    }
}


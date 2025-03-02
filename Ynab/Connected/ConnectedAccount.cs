using Ynab.Clients;
using Ynab.Responses.Accounts;

namespace Ynab.Connected;

public class ConnectedAccount(AccountsClient client, AccountResponse response) : Account(response)
{
    // ReSharper disable once UnusedMember.Local
    private readonly AccountsClient _client = client;
}
using Ynab.Clients;
using Ynab.Responses.Accounts;
using Ynab.Sanitisers;

namespace Ynab;

public class Account
{
    private readonly AccountsClient _accountsClient;
    private readonly AccountResponse _accountResponse;
    
    public string Name => _accountResponse.Name;
    public decimal Balance => MilliunitSanitiser.Calculate(_accountResponse.Balance);
    public bool OnBudget => _accountResponse.OnBudget;
    public bool Closed => _accountResponse.Closed;
    
    public Account(AccountsClient accountsClient, AccountResponse accountResponse)
    {
        _accountsClient = accountsClient;
        _accountResponse = accountResponse;
    }
}
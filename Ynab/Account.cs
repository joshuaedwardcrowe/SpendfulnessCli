using Ynab.Responses.Accounts;
using Ynab.Sanitisers;

namespace Ynab;

public class Account(AccountResponse response)
{
    public Guid Id => response.Id;
    public string Name => response.Name;
    public AccountType Type => response.Type;
    public decimal ClearedBalance => MilliunitSanitiser.Calculate(response.ClearedBalance);
    public bool Closed => response.Closed;
    public bool OnBudget => response.OnBudget;
}
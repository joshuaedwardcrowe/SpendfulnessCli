using Ynab.Sanitisers;

namespace Ynab;

public class NewAccount(string name, AccountType type, int balance)
{
    public string Name = name;
    public AccountType Type = type;
    public decimal Balance => MilliunitSanitiser.Desanitise(balance);
}
using Ynab;

namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommand : ICommand
{
    public const string CommandName = "spare-money";
}

// TODO: Move to ADR
// Domain decision - modifications to an aggregate to support viewmodel building should be executived via a superclass of the collection or aggregate, but preferably done via data modification if possible.
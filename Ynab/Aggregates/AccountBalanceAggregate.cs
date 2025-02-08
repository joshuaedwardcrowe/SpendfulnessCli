namespace Ynab.Aggregates;

public class AccountBalanceAggregate
{
    public string AccountName { get; set; }
    public decimal Balance { get; set; }
}

// TODO: my thinking is to use this aggregate, and have an optional 'balance to deduct' or 'ignore' fields for the spend money through category group.

// TODO: Move this to ADR later.
// THe domain rule with collections in Ynab is that they're typically containing wrappers. if no wrappers maybe it stays as aggregate.
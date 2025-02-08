using Ynab.Aggregates;

namespace Ynab.Extensions;

public static class AccountExtensions
{
    public static IEnumerable<Account> FilterToChecking(
        this IEnumerable<Account> accounts)
            => accounts.Where(x => x is { OnBudget: true, Closed: false });

    public static YnabAggregation<AccountBalanceAggregate> AggregateByBalance(this IEnumerable<Account> accounts)
    {
        var aggregation = accounts.Select(account => account.ToAggregate());

        return new YnabAggregation<AccountBalanceAggregate>
        {
            // TODO: Likely simplified by just using the constructor.
            Aggregation = aggregation,
        };
    }

    public static AccountBalanceAggregate ToAggregate(this Account account)
        => new()
        {
            AccountName = account.Name,
            Balance = account.Balance,
        };
}
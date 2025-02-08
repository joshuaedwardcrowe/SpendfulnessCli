using Ynab.Aggregates;

namespace YnabProgressConsole.Compilation.SpareMoney;

public static class YnabAggregationExtensions
{
    public static SpareMoneyAggregation IncludeAmountToIgnore(
        this YnabAggregation<AccountBalanceAggregate> aggregation, decimal amountToIgnore)
            => new()
                {
                    Aggregation = aggregation.Aggregation,
                    AmountToDeduct = amountToIgnore,
                };
}
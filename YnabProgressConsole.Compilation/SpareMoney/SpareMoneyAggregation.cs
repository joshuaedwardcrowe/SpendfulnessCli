using Ynab.Aggregates;

namespace YnabProgressConsole.Compilation.SpareMoney;

public class SpareMoneyAggregation : YnabAggregation<AccountBalanceAggregate>
{
    public decimal AmountToDeduct { get; set; }
}
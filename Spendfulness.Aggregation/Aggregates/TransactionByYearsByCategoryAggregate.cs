using YnabSharp.Collections;

namespace Spendfulness.Aggregation.Aggregates;

public record TransactionByYearsByCategoryAggregate(string CategoryName, IEnumerable<SplitTransactionsByYear> TransactionsByYears)
{
    public SplitTransactionsByYear TransactionsForYear(int year) => TransactionsByYears.First(tby => tby.Year == year);
    
    public decimal TotalAmountForYear(int year) => TransactionsForYear(year).TotalAmount;
}
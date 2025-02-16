namespace YnabProgressConsole.Compilation.Aggregates;

public record TransactionMonthFlaggedAggregate(
    string Month,
    List<TransactionMonthFlaggedAmountAggregate> AmountAggregates);
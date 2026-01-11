namespace Ynab.Collections;

public record SplitTransactionsByYear(int Year, IEnumerable<SplitTransactions> SplitTransactions);
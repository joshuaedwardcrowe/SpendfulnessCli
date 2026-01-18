namespace Ynab.Collections;

public record SplitTransactionsByYearByCategory(Guid CategoryId, IEnumerable<SplitTransactionsByYear> TransactionsByYear);
namespace SpendfulnessCli.Aggregation.Aggregates;

public record PotentialTransactionSplitAggregate(
    string OriginalTransactionId,
    string OriginalTransactionPayeeName,
    string OriginalTransactionCategoryName,
    string OriginalTransactionMemo,
    decimal OriginalTransactionAmount,
    string PotentialSplitPayeeName,
    string PotentialSplitCategoryName,
    string PotentialSplitMemo,
    decimal PotentialSplitAmount);
using Cli.Abstractions.Tables;

namespace SpendfulnessCli.CliTables.ViewModels;

public class PotentialTrasactionSplitCliTable : CliTable
{
    private static class ColumnNames
    {
        public const string SplittableTransactionId = "Original Guid";
        public const string SplittableTransactionPayeeName = "Original Payee";
        public const string SplittableTransactionCategoryName = "Original Category";
        public const string SplittableTransactionMemo = "Original Memo";
        public const string SplittableTransactionAmount = "Original Amount";
        public const string PotentialNewTransactionPayeeName = "Potential Payee Name";
        public const string PotentialNewTransactionCategoryName = "Potential Category Name";
        public const string PotentialNewTransactionMemo = "Potential Memo";
        public const string PotentialNewTransactionAmount = "Potential Amount";
    }
    
    // TODO: I don't like this as an abstraction. 
    // (it relies too much on developer awareness.
    public static List<string> GetColumnNames()
        => [
            ColumnNames.SplittableTransactionId,
            ColumnNames.PotentialNewTransactionPayeeName,
            ColumnNames.PotentialNewTransactionCategoryName,
            ColumnNames.PotentialNewTransactionMemo,
            ColumnNames.PotentialNewTransactionAmount
        ];
}
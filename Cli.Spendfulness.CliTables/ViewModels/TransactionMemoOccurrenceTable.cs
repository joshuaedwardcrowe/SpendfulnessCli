using Cli.Abstractions;

namespace Cli.Spendfulness.CliTables.ViewModels;

public class TransactionMemoOccurrenceTable : CliTable
{
    public const string PayeeColumnName = "Payee";
    public const string MemoColumnName = "Memo";
    public const string MemoOccurenceColumnName = "Occurence";
    public const string AverageAmountColumnName = "Average Amount";
    public const string TotalAmountColumnName = "Total Amount";
    
    public static List<string> GetColumnNames() 
        => [
            PayeeColumnName,
            MemoColumnName,
            MemoOccurenceColumnName,
            AverageAmountColumnName,
            TotalAmountColumnName
        ];
}
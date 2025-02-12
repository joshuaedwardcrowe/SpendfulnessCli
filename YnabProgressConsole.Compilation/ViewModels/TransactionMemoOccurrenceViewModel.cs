namespace YnabProgressConsole.Compilation.ViewModels;

public class TransactionMemoOccurrenceViewModel : ViewModel
{
    public const string PayeeColumnName = "Payee";
    public const string MemoColumnName = "Memo";
    public const string MemoOccurenceColumnName = "Occurence";
    public const string AverageAmountColumnName = "Average Amount";
    
    public static List<string> GetColumnNames() 
        => [
            PayeeColumnName,
            MemoColumnName,
            MemoOccurenceColumnName,
            AverageAmountColumnName
        ];
}
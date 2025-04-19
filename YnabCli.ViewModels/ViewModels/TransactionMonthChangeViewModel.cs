namespace YnabCli.ViewModels.ViewModels;

public class TransactionMonthChangeViewModel : ViewModel
{
    public const string MonthColumnName = "Yeear";
    public const string TotalAmountColumnName = "Total Amount";
    public const string PercentageChangeColumnName = "% Change";
    
    public static List<string> GetColumnNames() 
        => [MonthColumnName, TotalAmountColumnName, PercentageChangeColumnName];
}
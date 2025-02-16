namespace YnabProgressConsole.Compilation.ViewModels;

public class TransactionMonthFlaggedViewModel : ViewModel
{
    public const string MonthColumnName = "Month";

    public static List<string> GetColumnNames() => [MonthColumnName];
}
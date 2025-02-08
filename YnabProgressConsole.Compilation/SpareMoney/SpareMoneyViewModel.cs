namespace YnabProgressConsole.Compilation.SpareMoney;

public class SpareMoneyViewModel : ViewModel
{
    public const string SpareMoneyColumnName = "Spare Money";
    
    public static List<string> GetColumnNames() 
        => [SpareMoneyColumnName];
}
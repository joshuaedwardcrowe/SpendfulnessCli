namespace YnabProgressConsole.Compilation.SalaryIncreases;

public class AmountByYearViewModel : ViewModel
{
    public const string YearColumNName = "Yeear";
    public const string AverageAmountColumNName = "Average Amount";
    public const string PercentageIncreaseColumnNName = "% Increase";
    
    public static List<string> GetColumnNames() 
        => [YearColumNName, AverageAmountColumNName, PercentageIncreaseColumnNName];
}
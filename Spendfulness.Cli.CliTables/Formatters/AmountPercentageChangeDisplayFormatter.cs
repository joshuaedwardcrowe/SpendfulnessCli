namespace Spendfulness.Cli.CliTables.Formatters;

public static class AmountPercentageChangeDisplayFormatter
{
    public static string Format(decimal amount, int percentageChange)
    {
        var displayableAmount = CurrencyDisplayFormatter.Format(amount);
        var displayablePercentageChange = PercentageDisplayFormatter.Format(percentageChange);
        
        return percentageChange switch
        {
            > 0 => $"{displayableAmount} (+{displayablePercentageChange})",
            < 0 => $"{displayableAmount} ({displayablePercentageChange})", // Display will include a - anyway
            _ => $"{displayableAmount} ({displayablePercentageChange})"
        };
    }
}
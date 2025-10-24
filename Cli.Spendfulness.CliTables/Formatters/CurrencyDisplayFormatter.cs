namespace Cli.Spendfulness.CliTables.Formatters;

public static class CurrencyDisplayFormatter
{
    public static string Format(decimal currencyValue) => $"{currencyValue:C}";
}
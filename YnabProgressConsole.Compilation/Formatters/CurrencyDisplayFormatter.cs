namespace YnabProgressConsole.Compilation.Formatters;

public static class CurrencyDisplayFormatter
{
    public static string Format(decimal currencyValue) => $"{currencyValue:C}";
}
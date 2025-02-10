namespace YnabProgressConsole.Compilation;

public static class CurrencyDisplayFormatter
{
    public static string Format(decimal currencyValue)
        => $"{currencyValue:C}";
}
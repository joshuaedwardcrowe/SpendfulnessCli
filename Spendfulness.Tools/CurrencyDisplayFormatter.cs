namespace Spendfulness.Tools;

public static class CurrencyDisplayFormatter
{
    public static string Format(decimal currencyValue) => $"{currencyValue:C}";
}
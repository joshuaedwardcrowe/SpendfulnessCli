namespace Ynab.Sanitisers;

public static class DecimalSanitiser
{
    public static decimal Sanitise(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
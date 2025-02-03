using System.Globalization;

namespace Ynab.Generators;

public static class Identifier
{
    public static string ForMonth(DateTime date)
    {
        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
        return $"{monthName} {date.Year}";
    }
    
    public static string ForYear(DateTime date)
        => date.Year.ToString();
    
    public static string ForFlag(string? flagName, string? flagColour)
        => $"{flagName} ({flagColour})";
}
using System.Globalization;

namespace Ynab.Generators;

public static class IdentifierSanitiser
{
    public static string SanitiseForMonth(DateTime date)
    {
        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
        return $"{monthName} {date.Year}";
    }
    
    public static string SanitiseForYear(DateTime date)
        => date.Year.ToString();
    
    public static string SanitiseForFlag(string? flagName, string? flagColour)
        => $"{flagName} ({flagColour})";
}
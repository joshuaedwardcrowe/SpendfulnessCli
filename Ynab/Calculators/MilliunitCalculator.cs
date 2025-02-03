namespace Ynab.Calculators;

public static class MilliunitCalculator
{
    /// <summary>
    /// YNAB API stores currency in milliunit.
    /// </summary>
    /// <param name="milliunits"></param>
    /// <returns></returns>
    public static decimal Calculate(int milliunits) => milliunits / 1000.0m;
}
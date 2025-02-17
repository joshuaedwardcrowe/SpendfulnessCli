namespace Ynab.Sanitisers;

public static class MilliunitSanitiser
{
    private const decimal ConversationRate = 1000m;
    
    /// <summary>
    /// YNAB API stores currency in milliunit.
    /// </summary>
    /// <param name="milliunits"></param>
    /// <returns></returns>
    public static decimal Calculate(int milliunits) => milliunits / ConversationRate;
}
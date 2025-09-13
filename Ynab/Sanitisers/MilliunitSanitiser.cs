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

    public static decimal? Calculate(int? milliunits)
    {
        if (milliunits.HasValue)
            return Calculate(milliunits.Value);

        return null;
    }
    
    // TODO: This is a poor naming convention.
    public static int Desanitise(decimal currency)
    {
        var conversion = currency * ConversationRate;
        return (int)conversion;
    }
}
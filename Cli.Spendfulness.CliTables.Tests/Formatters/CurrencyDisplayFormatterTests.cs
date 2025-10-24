using Cli.Spendfulness.CliTables.Formatters;

namespace Cli.Spendfulness.CliTables.Tests.Formatters;

[TestFixture]
public class CurrencyDisplayFormatterTests
{
    [TestCase(0.1, "£0.10")]
    [TestCase(25, "£25.00")]
    [TestCase(45.23, "£45.23")]
    public void GivenCurrencyValue_WhenFormat_ReturnsFormattedCurrency(decimal currencyValue, string formattedCurrency)
    {
        var result = CurrencyDisplayFormatter.Format(currencyValue);
        
        Assert.That(result, Is.EqualTo(formattedCurrency));
    }
}
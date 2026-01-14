using Spendfulness.Formatters;

namespace Spendfulness.Cli.CliTables.Tests.Formatters;

[TestFixture]
public class PercentageDisplayFormatterTests
{
    [TestCase(10, "10%")]
    [TestCase(0.25, "0.25%")]
    [TestCase(100, "100%")]
    public void GivenPercentageValue_WhenFormat_ReturnsFormattedPercentage(decimal percentageValue, string formattedPercentage)
    {
        var result = PercentageDisplayFormatter.Format(percentageValue);
        
        Assert.That(result, Is.EqualTo(formattedPercentage));
    }
}
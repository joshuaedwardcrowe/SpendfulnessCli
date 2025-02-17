using Ynab.Sanitisers;

namespace Ynab.Tests.Sanitisers;

[TestFixture]
public class MilliunitSanitiserTests
{
    [Test]
    public void GivenMillunitValue_WhenCalculate_ConvertsToDecimal()
    {
        var expectedValue = 12.90m;
        var convertedToEquivelant = expectedValue * 1000m;
        var milliunitValue = (int)convertedToEquivelant;

        var result = MilliunitSanitiser.Calculate(milliunitValue);

        Assert.That(result, Is.EqualTo(expectedValue));
    }
}
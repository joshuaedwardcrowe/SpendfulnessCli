


using Spendfulness.Cli.Aggregation.Calculators;

namespace Spendfulness.Cli.CliTables.Tests.Calculators;

[TestFixture]
public class PercentageCalculatorTests
{
    [TestCase(50, 100, 100)]
    [TestCase(60, 40, -33)]
    [TestCase(40, 60, 50)]
    [TestCase(2340.20, 1043.20, -55)]
    public void GivenBigAndLowValue_WhenCalculateChange_Calculates(decimal oldValue, decimal newValue, int change)
    {
        var result = PercentageCalculator.CalculateChange(oldValue, newValue);
        
        Assert.That(result, Is.EqualTo(change));
    }
}
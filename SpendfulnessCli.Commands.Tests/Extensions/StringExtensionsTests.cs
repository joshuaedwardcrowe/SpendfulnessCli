using Cli.Commands.Abstractions.Extensions;

namespace Cli.Spendfulness.Commands.Tests.Extensions;

[TestFixture]
public class StringExtensionsTests
{
    [TestCase("ThisIsACommandTest", '-', "this-is-a-command-test")]
    [TestCase("ThisIsACommandTest", '/', "this/is/a/command/test")]
    public void GivenString_WhenToLowerSplitString_ReturnsSplitString(string input, char split, string expected)
    {
        var result = input.ToLowerSplitString(split);
        
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("ThisIsACommandTest", "tiact")]
    [TestCase("RecurringTransactions", "rt")]
    public void GivenString_WhenToLowerTitleCaseCharacters_ReturnsCharacters(string input, string expected)
    {
        var result = "".ToLowerTitleCharacters();
        
        Assert.That(result, Is.EqualTo(expected));
    }
}
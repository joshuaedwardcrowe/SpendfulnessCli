using Cli.Instructions.Indexers;
using NUnit.Framework;

namespace Cli.Instructions.Tests;

[TestFixture]
public class CliInstructionTokenIndexerTests
{
    private CliInstructionTokenIndexer _cliInstructionTokenIndexer;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _cliInstructionTokenIndexer = new CliInstructionTokenIndexer();
    }

    [Test]
    public void GivenNoInputString_WhenIndex_ReturnsFalseFlagsForAllIndexes()
    {
        var input = string.Empty;
        
        var result = _cliInstructionTokenIndexer.Index(input);
        
        Assert.That(result.PrefixTokenIndexed, Is.False);
        Assert.That(result.NameTokenIndexed, Is.False);
        Assert.That(result.SubNameTokenIndexed, Is.False);
        Assert.That(result.ArgumentTokensIndexed, Is.False);
    }
    
    [TestCase("command")]
    [TestCase("command-example")]
    [TestCase("command-example sub-command-example")]
    [TestCase("command-example sub-command-example --argument-example")]
    [TestCase("command-example sub-command-example --argument-example hello world")]
    [TestCase("command-example sub-command-example --argument-example hello world --argument-two")]
    [TestCase("command-example sub-command-example --argument-example hello world --argument-two 1")]
    public void GivenInputStringWithNoPrefix_WhenIndex_ReturnsFalseForFlag(string input)
    {
        var result = _cliInstructionTokenIndexer.Index(input);
        
        Assert.That(result.PrefixTokenIndexed, Is.False);
    }

    [TestCase("/")]
    [TestCase("/command")]
    [TestCase("/command-example")]
    [TestCase("/command-example sub-command-example")]
    [TestCase("/command-example sub-command-example --argument-example")]
    [TestCase("/command-example sub-command-example --argument-example hello world")]
    [TestCase("/command-example sub-command-example --argument-example hello world --argument-two")]
    [TestCase("/command-example sub-command-example --argument-example hello world --argument-two 1")]
    public void GivenInputString_WhenIndex_ReturnsCorrectIndexesForCommandPrefixToken(string input)
    {
        var result = _cliInstructionTokenIndexer.Index(input);
        
        Assert.That(result.PrefixTokenIndexed, Is.True);
        Assert.That(result.PrefixTokenStartIndex, Is.EqualTo(0));
        Assert.That(result.PrefixTokenEndIndex, Is.EqualTo(1));
    }

    [TestCase("/ ")]
    [TestCase("/ sub-command-example")]
    [TestCase("/ sub-command-example --argument-example")]
    [TestCase("/ sub-command-example --argument-example hello world")]
    [TestCase("/ sub-command-example --argument-example hello world --argument-two")]
    [TestCase("/ sub-command-example --argument-example hello world --argument-two 1")]
    public void GivenInputStringWithNoCommandNameToken_WhenIndex_ReturnsFalseForCommandNameTokenFlag(string input)
    {
        var result = _cliInstructionTokenIndexer.Index(input);
        
        Assert.That(result.NameTokenIndexed, Is.False);
    }

    [TestCase("/command-example", "command-example")]
    [TestCase("/command-example sub-command-example", "command-example")]
    [TestCase("/command-example sub-command-example --argument-example", "command-example")]
    [TestCase("/command-example sub-command-example --argument-example hello world", "command-example")]
    [TestCase("/command-example sub-command-example --argument-example hello world --argument-two", "command-example")]
    [TestCase("/command-example sub-command-example --argument-example hello world --argument-two 1", "command-example")]
    public void GivenInputString_WhenIndex_ReturnsCorrectIndexesForCommandNameToken(string input, string expectedMatch)
    {
        var result = _cliInstructionTokenIndexer.Index(input);
        
        Assert.That(result.NameTokenIndexed, Is.True);

        var match = input[result.NameTokenStartIndex..result.NameTokenEndIndex];
        Assert.That(match, Is.EqualTo(expectedMatch));
    }
    
    [TestCase("/command-example")]
    [TestCase("/command-example --argument-example")]
    [TestCase("/command-example --argument-example hello world")]
    [TestCase("/command-example --argument-example hello world --argument-two")]
    [TestCase("/command-example --argument-example hello world --argument-two 1")]
    public void GivenInputStringWithNoSubCommandNameToken_WhenIndex_ReturnsFalseForSubCommandNameTokenFlag(string input)
    {
        var result = _cliInstructionTokenIndexer.Index(input);
        
        Assert.That(result.SubNameTokenIndexed, Is.False);
    }
    
    [TestCase("/command-example sub-command-example", "sub-command-example")]
    [TestCase("/command-example sub-command-example --argument-example", "sub-command-example")]
    [TestCase("/command-example sub-command-example --argument-example hello world", "sub-command-example")]
    [TestCase("/command-example sub-command-example --argument-example hello world --argument-two", "sub-command-example")]
    [TestCase("/command-example sub-command-example --argument-example hello world --argument-two 1", "sub-command-example")]
    public void GivenInputString_WhenIndex_ReturnsCorrectIndexesForSubCommandNameToken(string input, string expectedMatch)
    {
        var result = _cliInstructionTokenIndexer.Index(input);
        
        Assert.That(result.SubNameTokenIndexed, Is.True);

        var match = input[result.SubNameStartIndex..result.SubNameEndIndex];
        Assert.That(match, Is.EqualTo(expectedMatch));
    }
    
    [TestCase("/command-example")]
    [TestCase("/command-example sub-command-example")]
    public void GivenInputStringWithNoArgumentTokens_WhenIndex_ReturnsFalseForArgumentTokensIndexed(string input)
    {
        var result = _cliInstructionTokenIndexer.Index(input);
        
        Assert.That(result.ArgumentTokensIndexed, Is.False);
    }
    
    [TestCase("/command-example sub-command-example --argument-example", "--argument-example")]
    [TestCase("/command-example sub-command-example --argument-example hello world", "--argument-example hello world")]
    [TestCase("/command-example sub-command-example --argument-example hello world --argument-two", "--argument-example hello world --argument-two")]
    [TestCase("/command-example sub-command-example --argument-example hello world --argument-two 1", "--argument-example hello world --argument-two 1")]
    [TestCase("/command-example sub-command-example --argument-example true --argument-two 1", "--argument-example true --argument-two 1")]
    [TestCase("/command-example sub-command-example --argument-example --argument-two 1", "--argument-example --argument-two 1")]
    public void GivenInputString_WhenIndex_ReturnsCorrectIndexesForArgumentTokens(string input, string expectedMatch)
    {
        var result = _cliInstructionTokenIndexer.Index(input);
        
        Assert.That(result.ArgumentTokensIndexed, Is.True);

        var match = input[result.ArgumentTokensStartIndex..result.ArgumentTokensEndIndex];
        Assert.That(match, Is.EqualTo(expectedMatch));
    }
}
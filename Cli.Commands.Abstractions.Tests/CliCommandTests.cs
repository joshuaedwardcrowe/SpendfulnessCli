using NUnit.Framework;

namespace Cli.Commands.Abstractions.Tests;

[TestFixture]
public class CliCommandTests
{
    private record TestCliCommand : CliCommand;
    private record AnotherCommand : CliCommand;
    
    [Test]
    public void GivenTestCliCommand_WhenGetCommandNameIsCalled_ThenCorrectNameIsReturned()
    {
        // Arrange
        var command = new TestCliCommand();
        
        // Act
        var commandName = command.GetSpecificCommandName();

        // Assert
        var expectedCommandName = nameof(TestCliCommand).Replace(nameof(CliCommand), string.Empty);
        Assert.That(commandName, Is.EqualTo(expectedCommandName));
    }
    
    [Test]
    public void GivenAnotherCliCommand_WhenGetCommandNameIsCalled_ThenCorrectNameIsReturned()
    {
        // Arrange
        var command = new AnotherCommand();
        
        // Act
        var commandName = command.GetSpecificCommandName();

        // Assert
        Assert.That(commandName, Is.EqualTo("AnotherCommand"));
    }
    
    [Test]
    [TestCase("SampleCliCommand", "sample")]
    [TestCase("SampleTwoCliCommand", "sample-two")]
    public void GivenFullCommandName_WhenStripInstructionName_InstructionNameIsReturned(string commandName, string expectedStrippedName)
    {
        // Act
        var strippedName = CliCommand.StripInstructionName(commandName);

        // Assert
        Assert.That(strippedName, Is.EqualTo(expectedStrippedName));
    }
}
using NUnit.Framework;

namespace Cli.Commands.Abstractions.Tests;

[TestFixture]
public class CliCommandTests
{
    public record TestCliCommand : CliCommand
    {
    }
    
    [Test]
    public void GivenTestCliCommand_WhenGetCommandNameIsCalled_ThenCorrectNameIsReturned()
    {
        // Arrange
        var command = new TestCliCommand();
        
        // Act
        var commandName = command.GetCommandName();

        // Assert
        var expectedCommandName = nameof(TestCliCommand).Replace(nameof(CliCommand), string.Empty);
        Assert.That(commandName, Is.EqualTo(expectedCommandName));
    }

    public record AnotherCommand : CliCommand
    {
    }
    
    [Test]
    public void GivenAnotherCliCommand_WhenGetCommandNameIsCalled_ThenCorrectNameIsReturned()
    {
        // Arrange
        var command = new AnotherCommand();
        
        // Act
        var commandName = command.GetCommandName();

        // Assert
        Assert.That(commandName, Is.EqualTo("AnotherCommand"));
    }
}
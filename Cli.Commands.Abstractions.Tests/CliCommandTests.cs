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
    public void GivenFullCommandName_WhenStripSpecificCommandNameIsCalled_ThenCorrectNameIsReturned()
    {
        // Arrange
        var fullCommandName = "SampleCliCommand";
        
        // Act
        var strippedName = CliCommand.StripSpecificCommandName(fullCommandName);

        // Assert
        var expectedStrippedName = fullCommandName.Replace(nameof(CliCommand), string.Empty);
        Assert.That(strippedName, Is.EqualTo(expectedStrippedName));
    }
}
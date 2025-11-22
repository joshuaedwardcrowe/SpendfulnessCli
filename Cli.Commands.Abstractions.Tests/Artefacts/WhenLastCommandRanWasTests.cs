using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Artefacts.CommandRan;
using NUnit.Framework;

namespace Cli.Commands.Abstractions.Tests.Artefacts;

[TestFixture]
public class WhenLastCommandRanWasTests
{
    private record TestCliCommand : CliCommand;
    
    [Test]
    public void GivenRanCommandArtefact_WhenLastCommandRanWas_ThenShouldReturnTrue()
    {
        // Arrange
        var properties = new List<CliCommandArtefact>
        {
            new CliCommandRanArtefact(new TestCliCommand())
        };
        
        // Act
        var result = properties.LastCommandRanWas<TestCliCommand>();
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void GivenNoRanCommandArtefact_WhenLastCommandRanWas_ThenShouldReturnFalse()
    {
        // Arrange
        var properties = new List<CliCommandArtefact>();
        
        // Act
        var result = properties.LastCommandRanWas<TestCliCommand>();
        
        // Assert
        Assert.That(result, Is.False);
    }
}
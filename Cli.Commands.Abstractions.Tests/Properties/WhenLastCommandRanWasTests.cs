using Cli.Commands.Abstractions.Properties;
using Cli.Commands.Abstractions.Properties.CommandRan;
using NUnit.Framework;

namespace Cli.Commands.Abstractions.Tests.Properties;

[TestFixture]
public class WhenLastCommandRanWasTests
{
    private record TestCliCommand : CliCommand;
    
    [Test]
    public void GivenRanCommandProperty_WhenLastCommandRanWas_ThenShouldReturnTrue()
    {
        // Arrange
        var properties = new List<CliCommandProperty>
        {
            new CliCommandRanProperty(new TestCliCommand())
        };
        
        // Act
        var result = properties.LastCommandRanWas<TestCliCommand>();
        
        // Assert
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void GivenNoRanCommandProperty_WhenLastCommandRanWas_ThenShouldReturnTrue()
    {
        // Arrange
        var properties = new List<CliCommandProperty>();
        
        // Act
        var result = properties.LastCommandRanWas<TestCliCommand>();
        
        // Assert
        Assert.That(result, Is.False);
    }
}
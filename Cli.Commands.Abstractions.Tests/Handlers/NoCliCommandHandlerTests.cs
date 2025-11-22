using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes.Final;
using NUnit.Framework;

namespace Cli.Commands.Abstractions.Tests.Handlers;

[TestFixture]
public class NoCliCommandHandlerTests
{
    private record TestCliCommand : CliCommand;
    
    private class TestCliCommandHandler : NoCliCommandHandler<TestCliCommand>;
    
    private NoCliCommandHandler<TestCliCommand> _classUnderTest;

    [SetUp]
    public void SetUp()
    {
        _classUnderTest = new TestCliCommandHandler();
    }
    
    [Test]
    public async Task GivenCommand_WhenHandle_ReturnsOutputOutcome()
    {
        // Arrange
        var command = new TestCliCommand();
        
        // Act
        var outcome = await _classUnderTest.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(outcome.Length, Is.EqualTo(1));
        
        var outputOutcome = outcome[0] as CliCommandOutputOutcome;
        Assert.That(outputOutcome, Is.Not.Null);
        Assert.That(outputOutcome.Output, Is.Not.Null);
    }
}
using Cli.Workflow.Abstractions;
using NUnit.Framework;

namespace Cli.Workflow.Tests;

[TestFixture]
public class CliWorkflowRunStateStopwatchTests : CliWorkflowRunStateTests
{
    [Test]
    public void GivenStateIsCreated_WhenChangeToRunning_StartsStopwatch()
    {
        // Arrange
        var state = GetPreparedState([ClIWorkflowRunStateType.Created]);
        
        // Act
        state.ChangeTo(ClIWorkflowRunStateType.Running);
        
        // Assert
        Assert.That(state.Stopwatch.IsRunning, Is.True);
    }

    [Test]
    public void GivenStateIsRunning_WhenChangeToFinished_StopsStopwatch()
    {
        // Arrange
        var state = GetPreparedState([ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.InvalidAsk]);
        
        // Act
        state.ChangeTo(ClIWorkflowRunStateType.Finished);
        
        // Assert
        Assert.That(state.Stopwatch.IsRunning, Is.False);
    }
}
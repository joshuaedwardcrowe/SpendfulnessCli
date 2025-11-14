using Cli.Workflow.Abstractions;
using NUnit.Framework;

namespace Cli.Workflow.Tests;

[TestFixture]
public class CliWorkflowRunStateValidStateChangeTests
{
    public static IEnumerable<TestCaseData> ValidStateChanges()
    {
        yield return new TestCaseData(
            Enumerable.Empty<ClIWorkflowRunStateType>(),
            ClIWorkflowRunStateType.Created
        ).SetName("State can be changed from NotInitialized to Created");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Created },
            ClIWorkflowRunStateType.Running
        ).SetName("State can be changed from Created to Running");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running },
            ClIWorkflowRunStateType.InvalidAsk
        ).SetName("State can be changed from Running to InvalidAsk");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.InvalidAsk },
            ClIWorkflowRunStateType.Finished
        ).SetName("State can be changed from InvalidAsk to Finished");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running },
            ClIWorkflowRunStateType.Exceptional
        ).SetName("State can be changed from Running to Exceptional");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.Exceptional },
            ClIWorkflowRunStateType.Finished
        ).SetName("State can be changed from Exceptional to Finished");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running },
            ClIWorkflowRunStateType.Finished
        ).SetName("State can be changed from Running to Finished");
    }
    
    [TestCaseSource(nameof(ValidStateChanges))]
    public void GivenStateIs_WhenChangeTo_CanBeChanged(IEnumerable<ClIWorkflowRunStateType> priorStates, ClIWorkflowRunStateType stateToChangeTo)
    {
        // Arrange
        var state = new CliWorkflowRunState();

        foreach (var priorState in priorStates)
        {
            state.ChangeTo(priorState);
        }
        
        // Act & Assert
        Assert.DoesNotThrow(() => state.ChangeTo(stateToChangeTo));
    }
    
    [TestCaseSource(nameof(ValidStateChanges))]
    public void GivenStateIsNotInitialized_WhenChangeToCreated_RecordsStateChange(IEnumerable<ClIWorkflowRunStateType> priorStates, ClIWorkflowRunStateType stateToChangeTo)
    {
        // Arrange
        var state = new CliWorkflowRunState();
        
        foreach (var priorState in priorStates)
        {
            state.ChangeTo(priorState);
        }
        
        // Act
        state.ChangeTo(stateToChangeTo);
        
        // Assert
        var priorStateChange = priorStates.Any() ? priorStates.Last() : ClIWorkflowRunStateType.NotInitialized;
        var stateChange = state.Changes.Last();
        
        Assert.That(stateChange, Is.Not.Null);
        Assert.That(stateChange!.StartedAt, Is.EqualTo(priorStateChange));
        Assert.That(stateChange.MovedTo, Is.EqualTo(stateToChangeTo));
    }
}
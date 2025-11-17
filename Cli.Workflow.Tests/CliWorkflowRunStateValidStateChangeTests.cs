using Cli.Workflow.Abstractions;
using NUnit.Framework;

namespace Cli.Workflow.Tests;

[TestFixture]
public class CliWorkflowRunStateValidStateChangeTests : CliWorkflowRunStateTests
{
    public static IEnumerable<TestCaseData> ValidStateChanges()
    {
        yield return new TestCaseData(
            Array.Empty<ClIWorkflowRunStateType>(),
            ClIWorkflowRunStateType.Running
        ).SetName("State can be changed from Created to Running");
        
        yield return new TestCaseData(
            Array.Empty<ClIWorkflowRunStateType>(),
            ClIWorkflowRunStateType.InvalidAsk
        ).SetName("State can be changed from Created to InvalidAsk");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Running },
            ClIWorkflowRunStateType.InvalidAsk
        ).SetName("State can be changed from Running to InvalidAsk");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.InvalidAsk },
            ClIWorkflowRunStateType.Finished
        ).SetName("State can be changed from InvalidAsk to Finished");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Running },
            ClIWorkflowRunStateType.Exceptional
        ).SetName("State can be changed from Running to Exceptional");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.Exceptional },
            ClIWorkflowRunStateType.Finished
        ).SetName("State can be changed from Exceptional to Finished");
        
        yield return new TestCaseData(
            new[] { ClIWorkflowRunStateType.Running },
            ClIWorkflowRunStateType.Finished
        ).SetName("State can be changed from Running to Finished");
    }
    
    [TestCaseSource(nameof(ValidStateChanges))]
    public void GivenStateIs_WhenChangeTo_CanBeChanged(IEnumerable<ClIWorkflowRunStateType> priorStates, ClIWorkflowRunStateType stateToChangeTo)
    {
        // Arrange
        var state = GetPreparedState(priorStates);
        
        // Act & Assert
        Assert.DoesNotThrow(() => state.ChangeTo(stateToChangeTo));
    }
    
    [TestCaseSource(nameof(ValidStateChanges))]
    public void GivenStateIsNotInitialized_WhenChangeToCreated_RecordsStateChange(ClIWorkflowRunStateType[] priorStates, ClIWorkflowRunStateType stateToChangeTo)
    {
        // Arrange
        var state = GetPreparedState(priorStates);
        
        // Act
        state.ChangeTo(stateToChangeTo);
        
        // Assert
        var priorStateChange = priorStates.Any() ? priorStates.Last() : ClIWorkflowRunStateType.Created;
        var stateChange = state.Changes.Last();
        
        Assert.That(stateChange, Is.Not.Null);
        Assert.That(stateChange!.From, Is.EqualTo(priorStateChange));
        Assert.That(stateChange.To, Is.EqualTo(stateToChangeTo));
    }
}
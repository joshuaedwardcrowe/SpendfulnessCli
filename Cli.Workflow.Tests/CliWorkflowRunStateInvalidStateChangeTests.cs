using Cli.Workflow.Abstractions;
using NUnit.Framework;

namespace Cli.Workflow.Tests;

public class CliWorkflowRunStateInvalidStateChangeTests
{
    public static IEnumerable<TestCaseData> InvalidStateChanges()
    {
        yield return new TestCaseData(
            Enumerable.Empty<ClIWorkflowRunStateType>(),
            ClIWorkflowRunStateType.NotInitialized
        ).SetName("State cannot be NotYetInitialized when already NotInitialized");
        
        yield return new TestCaseData(
            new [] { ClIWorkflowRunStateType.Created },
            ClIWorkflowRunStateType.Created
        ).SetName("State cannot be Created when already Created");
        
        yield return new TestCaseData(
            Enumerable.Empty<ClIWorkflowRunStateType>(),
            ClIWorkflowRunStateType.Running
        ).SetName("State cannot be Running when not NotInitialized");
        
        yield return new TestCaseData(
            new [] { ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running },
            ClIWorkflowRunStateType.Running
        ).SetName("State cannot be Running when already Running");
        
        yield return new TestCaseData(
            Enumerable.Empty<ClIWorkflowRunStateType>(),
            ClIWorkflowRunStateType.InvalidAsk
        ).SetName("State cannot be InvalidAsk when not NotInitialized");
        
        yield return new TestCaseData(
            new [] { ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.InvalidAsk},
            ClIWorkflowRunStateType.InvalidAsk
        ).SetName("State cannot be InvalidAsk when already InvalidAsk");
        
        yield return new TestCaseData(
            Enumerable.Empty<ClIWorkflowRunStateType>(),
            ClIWorkflowRunStateType.Exceptional
        ).SetName("State cannot be Exceptional when not NotInitialized");
        
        yield return new TestCaseData(
            new [] { ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.Exceptional },
            ClIWorkflowRunStateType.Exceptional
        ).SetName("State cannot be InvalidAsk when already Exceptional");
        
        yield return new TestCaseData(
            Enumerable.Empty<ClIWorkflowRunStateType>(),
            ClIWorkflowRunStateType.Finished
        ).SetName("State cannot be Finished when not NotInitialized");
        
        yield return new TestCaseData(
            new [] { ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.Finished },
            ClIWorkflowRunStateType.Finished
        ).SetName("State cannot be Finished when already Finished");
    }
    
    
    [TestCaseSource(nameof(InvalidStateChanges))]
    public void GivenStateIs_WhenChangedTo_CannotBeChanged(IEnumerable<ClIWorkflowRunStateType> priorStates, ClIWorkflowRunStateType stateToChangeTo)
    {
        var state = new CliWorkflowRunState();

        foreach (var priorState in priorStates)
        {
            state.ChangeTo(priorState);
        }
        
        // Act & Assert
        Assert.Throws<ImpossibleStateChangeException>(() => state.ChangeTo(stateToChangeTo));
    }
}
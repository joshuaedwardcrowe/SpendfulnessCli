using Cli.Workflow.Abstractions;

namespace Cli.Workflow;

public class CliWorkflowRunState
{
    private readonly List<RecordedCliWorkflowRunStateChange> _recordedStateChanges = [];

    public void ChangeTo(ClIWorkflowRunStateType stateTypeToChangeTo)
    {
        var mostRecentState = _recordedStateChanges.LastOrDefault();
        var currentState = mostRecentState?.MovedTo ?? ClIWorkflowRunStateType.NotInitialized;
        
        // Can chnge from most recently changed to, to new state to change to.
        var possibleStateChange = PossibleStateChanges
            .Any(cliWorkflowRunStateChange =>
                cliWorkflowRunStateChange.StartedAt == currentState && 
                cliWorkflowRunStateChange.MovedTo == stateTypeToChangeTo);

        if (!possibleStateChange)
        {
            throw new ImpossibleStateChangeException($"Invalid state change: {currentState} > {stateTypeToChangeTo}");
        }

        var newState = new RecordedCliWorkflowRunStateChange(DateTime.UtcNow, currentState, stateTypeToChangeTo);
        _recordedStateChanges.Add(newState);
    }

    // TODO: CLI - Does this matter at all?
    private static readonly List<CliWorkflowRunStateChange> PossibleStateChanges =
    [
        new(ClIWorkflowRunStateType.NotInitialized, ClIWorkflowRunStateType.Created),
        new(ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running),
        
        new(ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.InvalidAsk),
        new(ClIWorkflowRunStateType.InvalidAsk, ClIWorkflowRunStateType.Finished),
        
        new(ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.Exceptional),
        new(ClIWorkflowRunStateType.Exceptional, ClIWorkflowRunStateType.Finished),
        
        new(ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.Finished)
    ];
}
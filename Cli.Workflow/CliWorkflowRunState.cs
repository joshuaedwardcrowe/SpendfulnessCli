using System.Diagnostics;
using Cli.Workflow.Abstractions;

namespace Cli.Workflow;

public class CliWorkflowRunState
{
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly List<RecordedCliWorkflowRunStateChange> _recordedStateChanges = [];

    public void ChangeTo(ClIWorkflowRunStateType stateTypeToChangeTo)
    {
        var currentState = CanChangeTo(stateTypeToChangeTo);
        
        UpdateStopwatch(stateTypeToChangeTo);

        var stateChange = new RecordedCliWorkflowRunStateChange(
            _stopwatch.ElapsedTicks,
            currentState, 
            stateTypeToChangeTo);
        
        _recordedStateChanges.Add(stateChange);
    }

    private ClIWorkflowRunStateType CanChangeTo(ClIWorkflowRunStateType stateTypeToChangeTo)
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
        
        return currentState;
    }

    private void UpdateStopwatch(ClIWorkflowRunStateType stateTypeToChangeTo)
    {
        if (stateTypeToChangeTo != ClIWorkflowRunStateType.Running)
        {
            _stopwatch.Start();
        }

        if (stateTypeToChangeTo == ClIWorkflowRunStateType.Finished)
        {
            _stopwatch.Stop();
        }
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
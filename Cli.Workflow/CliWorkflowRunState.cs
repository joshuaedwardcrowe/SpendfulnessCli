using System.Diagnostics;
using Cli.Workflow.Abstractions;

namespace Cli.Workflow;

// TODO: Write unit tests.
public class CliWorkflowRunState
{
    public readonly Stopwatch Stopwatch = new Stopwatch();
    public readonly List<RecordedCliWorkflowRunStateChange> Changes = [];

    public void ChangeTo(ClIWorkflowRunStateType stateTypeToChangeTo)
    {
        var currentState = CanChangeTo(stateTypeToChangeTo);
        
        UpdateStopwatch(stateTypeToChangeTo);

        var stateChange = new RecordedCliWorkflowRunStateChange(
            Stopwatch.ElapsedTicks,
            currentState, 
            stateTypeToChangeTo);
        
        Changes.Add(stateChange);
    }

    public bool Is(ClIWorkflowRunStateType suggestedStateType)
    {
        // TODO: Clean this up.
        return Changes.LastOrDefault()?.MovedTo == suggestedStateType;
    }

    private ClIWorkflowRunStateType CanChangeTo(ClIWorkflowRunStateType stateTypeToChangeTo)
    {
        var mostRecentState = Changes.LastOrDefault();
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
            Stopwatch.Start();
        }

        if (stateTypeToChangeTo == ClIWorkflowRunStateType.Finished)
        {
            Stopwatch.Stop();
        }
    }

    /// <summary>
    /// Stops a CliWorkflowRun from being re-eexecuted for another command.
    /// </summary>
    private static readonly List<CliWorkflowRunStateChange> PossibleStateChanges =
    [
        new(ClIWorkflowRunStateType.NotInitialized, ClIWorkflowRunStateType.Created),
        new(ClIWorkflowRunStateType.Created, ClIWorkflowRunStateType.Running),
        
        // This exists to handle multi-turn commands.
        new(ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.CanContinue),
        new(ClIWorkflowRunStateType.CanContinue, ClIWorkflowRunStateType.Running),
        
        new(ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.InvalidAsk),
        new(ClIWorkflowRunStateType.InvalidAsk, ClIWorkflowRunStateType.Finished),
        
        new(ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.Exceptional),
        new(ClIWorkflowRunStateType.Exceptional, ClIWorkflowRunStateType.Finished),
        
        new(ClIWorkflowRunStateType.Running, ClIWorkflowRunStateType.Finished)
    ];
}
using Cli.Workflow.Abstractions;

namespace Cli;

public class CliWorkflowRunStateManager
{
    private readonly List<CliWorkflowRunStateChange> _stateChanges = [];

    public void ChangeTo(ClIWorkflowRunState stateToChangeTo)
    {
        var mostRecentState = _stateChanges.LastOrDefault();
        var currentState = mostRecentState?.To ?? ClIWorkflowRunState.NotInitialized;
        
        // Can chnge from most recently changed to, to new state to change to.
        var possibleStateChange = PossibleStateChanges
            .Any(cliWorkflowRunStateChange =>
                cliWorkflowRunStateChange.From == currentState && 
                cliWorkflowRunStateChange.To == stateToChangeTo);

        if (!possibleStateChange)
        {
            throw new ImpossibleStateChangeException($"Invalid state change: {currentState} > {stateToChangeTo}");
        }

        var newState = new CliWorkflowRunStateChange(currentState, stateToChangeTo);
        _stateChanges.Add(newState);
    }

    private static List<CliWorkflowRunStateChange> PossibleStateChanges =
    [
        new(ClIWorkflowRunState.Created, ClIWorkflowRunState.Running),
        new(ClIWorkflowRunState.Running, ClIWorkflowRunState.Finished)
    ];
}
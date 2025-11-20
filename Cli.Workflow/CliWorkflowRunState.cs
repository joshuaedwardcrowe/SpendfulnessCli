using System.Diagnostics;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Instructions.Abstractions;
using Cli.Workflow.Abstractions;

namespace Cli.Workflow;

public class CliWorkflowRunState
{
    public readonly Stopwatch Stopwatch = new Stopwatch();
    public readonly List<CliWorkflowRunStateChange> Changes = [];
    
    public bool WasChangedTo(ClIWorkflowRunStateStatus status)
    {
        return Changes.Any(change => change.To == status);
    }
    
    public void ChangeTo(ClIWorkflowRunStateStatus statusToChangeTo)
    {
        var priorState = CanChangeTo(statusToChangeTo);
        
        UpdateStopwatch(statusToChangeTo);

        var stateChange = new CliWorkflowRunStateChange(
            Stopwatch.Elapsed,
            priorState, 
            statusToChangeTo);
        
        Changes.Add(stateChange);
    }

    public void ChangeTo(ClIWorkflowRunStateStatus statusToChangeTo, CliInstruction instruction)
    {
        var priorState = CanChangeTo(statusToChangeTo);
        
        UpdateStopwatch(statusToChangeTo);
        
        var stateChange = new InstructionCliWorkflowRunStateChange(
            Stopwatch.Elapsed,
            priorState, 
            statusToChangeTo,
            instruction);
        
        Changes.Add(stateChange);
    }
    
    public void ChangeTo(ClIWorkflowRunStateStatus statusToChangeTo, CliCommandOutcome[] outcomes)
    {
        var priorState = CanChangeTo(statusToChangeTo);
        
        UpdateStopwatch(statusToChangeTo);
        
        var stateChange = new OutcomeCliWorkflowRunStateChange(
            Stopwatch.Elapsed,
            priorState, 
            statusToChangeTo,
            outcomes);
        
        Changes.Add(stateChange);
    }

    private ClIWorkflowRunStateStatus CanChangeTo(ClIWorkflowRunStateStatus stateStatusToChangeTo)
    {
        var mostRecentState = Changes.LastOrDefault();
        var priorState = mostRecentState?.To ?? ClIWorkflowRunStateStatus.Created;
        
        // Can chnge from most recently changed to, to new state to change to.
        var possibleStateChange = PossibleStateChanges
            .Any(cliWorkflowRunStateChange =>
                cliWorkflowRunStateChange.IfStartedAt == priorState && 
                cliWorkflowRunStateChange.CanMoveTo == stateStatusToChangeTo);

        if (!possibleStateChange)
        {
            throw new ImpossibleStateChangeException($"Invalid state change: {priorState} > {stateStatusToChangeTo}");
        }
        
        return priorState;
    }

    private void UpdateStopwatch(ClIWorkflowRunStateStatus stateStatusToChangeTo)
    {
        if (stateStatusToChangeTo == ClIWorkflowRunStateStatus.Running)
        {
            Stopwatch.Start();
        }

        if (stateStatusToChangeTo == ClIWorkflowRunStateStatus.Finished)
        {
            Stopwatch.Stop();
        }
    }

    /// <summary>
    /// Stops a CliWorkflowRun from being re-eexecuted for another command.
    /// </summary>
    private static readonly List<PossibleCliWorkflowRunStateChange> PossibleStateChanges =
    [
        new(ClIWorkflowRunStateStatus.Created, ClIWorkflowRunStateStatus.InvalidAsk),
        new(ClIWorkflowRunStateStatus.Created, ClIWorkflowRunStateStatus.Running),
        
        new(ClIWorkflowRunStateStatus.Running, ClIWorkflowRunStateStatus.InvalidAsk),
        new(ClIWorkflowRunStateStatus.InvalidAsk, ClIWorkflowRunStateStatus.Finished),
        
        new(ClIWorkflowRunStateStatus.Running, ClIWorkflowRunStateStatus.Exceptional),
        new(ClIWorkflowRunStateStatus.Exceptional, ClIWorkflowRunStateStatus.Finished),
        
        new(ClIWorkflowRunStateStatus.Running, ClIWorkflowRunStateStatus.ReachedReusableOutcome),
        new(ClIWorkflowRunStateStatus.ReachedReusableOutcome, ClIWorkflowRunStateStatus.ReachedReusableOutcome),
        new(ClIWorkflowRunStateStatus.ReachedReusableOutcome, ClIWorkflowRunStateStatus.ReachedFinalOutcome),
        
        new(ClIWorkflowRunStateStatus.Running, ClIWorkflowRunStateStatus.ReachedFinalOutcome),
        new(ClIWorkflowRunStateStatus.ReachedFinalOutcome, ClIWorkflowRunStateStatus.Finished),
    ];
}
using Cli.Commands.Abstractions.Outcomes.Reusable;
using Cli.Workflow.Abstractions;
using Cli.Workflow.Run.State;

namespace Cli.Workflow.Tests.Run.State;

public abstract class CliWorkflowRunStateTests
{
    protected static CliWorkflowRunState GetPreparedState(IEnumerable<ClIWorkflowRunStateStatus> priorStatuses)
    {
        var state = new CliWorkflowRunState();
        
        foreach (var priorStatus in priorStatuses)
        {
            ChangeStateStatus(state, priorStatus);
        }

        return state;
    }

    private static void ChangeStateStatus(CliWorkflowRunState state, ClIWorkflowRunStateStatus status)
    {
        if (status is ClIWorkflowRunStateStatus.ReachedReusableOutcome)
        {
            var reusableOutcome = new CliCommandMessageOutcome(
                nameof(ClIWorkflowRunStateStatus.ReachedReusableOutcome));
                
            state.ChangeTo(status, [reusableOutcome]);
            return;
        }

        state.ChangeTo(status);
    }
}
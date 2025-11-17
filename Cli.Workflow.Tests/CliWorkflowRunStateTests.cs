using Cli.Workflow.Abstractions;

namespace Cli.Workflow.Tests;

public abstract class CliWorkflowRunStateTests
{
    protected static CliWorkflowRunState GetPreparedState(IEnumerable<ClIWorkflowRunStateStatus> priorStatuses)
    {
        var state = new CliWorkflowRunState();
        
        foreach (var priorStatus in priorStatuses)
        {
            state.ChangeTo(priorStatus);
        }

        return state;
    }
}
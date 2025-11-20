using Cli.Workflow.Abstractions;

namespace Cli.Workflow.Run.State.Change;

public class CliWorkflowRunStateChange
{
    public readonly TimeSpan At;
    public readonly ClIWorkflowRunStateStatus From;
    public readonly ClIWorkflowRunStateStatus To;
    
    public CliWorkflowRunStateChange(
        TimeSpan at,
        ClIWorkflowRunStateStatus from,
        ClIWorkflowRunStateStatus to)
    {
        At = at;
        From = from;
        To = to;
    }
}
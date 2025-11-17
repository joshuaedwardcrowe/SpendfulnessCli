using Cli.Workflow.Abstractions;

namespace Cli.Workflow;

public class CliWorkflowRunStateChange
{
    public readonly TimeSpan At;
    public readonly ClIWorkflowRunStateType From;
    public readonly ClIWorkflowRunStateType To;
    
    public CliWorkflowRunStateChange(
        TimeSpan at,
        ClIWorkflowRunStateType from,
        ClIWorkflowRunStateType to)
    {
        At = at;
        From = from;
        To = to;
    }
}
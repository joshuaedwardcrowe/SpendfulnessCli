using Cli.Workflow.Abstractions;

namespace Cli.Workflow;


public class CliWorkflowRunStateChange(ClIWorkflowRunStateType StartedAt, ClIWorkflowRunStateType MovedTo)
{
    public readonly ClIWorkflowRunStateType StartedAt = StartedAt;
    public readonly ClIWorkflowRunStateType MovedTo = MovedTo;
}

public class RecordedCliWorkflowRunStateChange(
    DateTime changedAt,
    ClIWorkflowRunStateType startedAt,
    ClIWorkflowRunStateType movedTo)
    : CliWorkflowRunStateChange(startedAt, movedTo)
{
    public readonly DateTime ChangedAt = changedAt;
}

    
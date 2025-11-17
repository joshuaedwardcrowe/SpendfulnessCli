namespace Cli.Workflow.Abstractions;

public enum ClIWorkflowRunStateStatus
{
    Created,
    Running,
    InvalidAsk,
    Exceptional,
    ReachedFinalOutcome,
    ReachedReusableOutcome,
    Finished,
}
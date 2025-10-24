namespace Cli.Workflow.Abstractions;

public enum ClIWorkflowRunStateType
{
    NotInitialized,
    Created,
    Running,
    InvalidAsk,
    Exceptional,
    Finished,
}
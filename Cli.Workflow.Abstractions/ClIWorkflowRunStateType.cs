namespace Cli.Workflow.Abstractions;

// TODO: Rename to CliWorkflowRunStateStatus
public enum ClIWorkflowRunStateType
{
    NotInitialized,
    Created,
    Running,
    // TODO: There might be a better name
    CanContinue,
    InvalidAsk,
    Exceptional,
    Finished,
}
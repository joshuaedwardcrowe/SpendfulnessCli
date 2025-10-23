namespace YnabCli;

public enum ClIWorkflowRunState
{
    // TODO: better name?
    NotYetRunning,
    Running,
    Stopped,
    
    NoInput,
    NoCommand,
    Exceptional,
}
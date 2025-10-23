namespace YnabCli;

public abstract class CliWorkflowRunOutcome
{
    protected CliWorkflowRunOutcomeKind Kind { get;  set; }
    
    protected CliIo CliIo { get;  set; }

    public CliWorkflowRunOutcome(CliWorkflowRunOutcomeKind kind, CliIo cliIo)
    {
        Kind = kind;
        CliIo = cliIo;
    }
}
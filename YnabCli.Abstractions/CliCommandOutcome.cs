namespace YnabCli.Abstractions;

public abstract class CliCommandOutcome(CliWorkflowRunOutcomeKind kind)
{
    public CliWorkflowRunOutcomeKind Kind { get;  set; } = kind;
}

// TODO: Implement a JsonOutputOutcome?
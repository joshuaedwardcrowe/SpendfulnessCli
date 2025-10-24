namespace Cli.Outcomes;

public abstract class CliCommandOutcome(CliWorkflowRunOutcomeKind kind)
{
    public CliWorkflowRunOutcomeKind Kind { get; } = kind;
}

// TODO: Implement a JsonOutputOutcome?
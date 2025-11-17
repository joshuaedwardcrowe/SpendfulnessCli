namespace Cli.Commands.Abstractions.Outcomes;

public abstract class CliCommandOutcome
{
    public CliCommandOutcomeKind Kind { get; }
    
    protected CliCommandOutcome(CliCommandOutcomeKind kind)
    {
        Kind = kind;
    }
}
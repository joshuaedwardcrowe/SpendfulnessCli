namespace Cli.Commands.Abstractions.Outcomes;

public abstract class CliCommandOutcome(CliCommandOutcomeKind kind)
{
    public CliCommandOutcomeKind Kind { get; } = kind;
}
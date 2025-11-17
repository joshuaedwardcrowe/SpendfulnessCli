namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandExceptionOutcome(Exception exception) : CliCommandOutcome(CliCommandOutcomeKind.Exception)
{
    public Exception Exception { get; set; } = exception;
}
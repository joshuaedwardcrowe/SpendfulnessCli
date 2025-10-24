using Cli.Outcomes;

namespace Cli;

public class CliCommandExceptionOutcome(Exception exception) : CliCommandOutcome(CliWorkflowRunOutcomeKind.Exception)
{
    public Exception Exception { get; set; } = exception;
}
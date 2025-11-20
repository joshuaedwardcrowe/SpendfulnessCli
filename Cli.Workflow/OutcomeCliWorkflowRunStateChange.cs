using Cli.Commands.Abstractions.Outcomes;
using Cli.Workflow.Abstractions;

namespace Cli.Workflow;

public class OutcomeCliWorkflowRunStateChange(
    TimeSpan at,
    ClIWorkflowRunStateStatus from,
    ClIWorkflowRunStateStatus to,
    CliCommandOutcome[] outcomes)
    : CliWorkflowRunStateChange(at, from, to)
{
    public readonly CliCommandOutcome[] Outcomes = outcomes;
}
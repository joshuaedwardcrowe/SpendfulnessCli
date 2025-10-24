using Cli.Commands.Abstractions.Io;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Workflow;

namespace Cli.Spendfulness;

public class YnabCliApp : CliApp
{
    public YnabCliApp(CliWorkflow cliWorkflow, CliCommandOutcomeIo cliCommandOutcomeIo)
        : base(cliWorkflow, cliCommandOutcomeIo)
    {
    }

    protected override void OnRun(CliWorkflow cliWorkflow, CliIo cliIo)
    {
        cliIo.Say($"New world CLI started");
    }

    protected override void OnRunCreated(CliWorkflowRun cliWorkflowRun, CliIo cliIo)
    {
        cliIo.Say($"New world CLI run created");
    }

    protected override void OnRunStarted(CliWorkflowRun cliWorkflowRun, CliIo cliIo)
    {
        cliIo.Say($"New world CLI run started");
    }
}
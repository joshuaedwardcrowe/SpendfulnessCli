using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;

namespace Cli.Workflow.Commands;

public class ExitCliCommandHandler(CliWorkflow cliWorkflow) : CliCommandHandler, ICliCommandHandler<ExitCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(ExitCliCommand command, CancellationToken cancellationToken)
    {
        cliWorkflow.Stop();
        
        var outcome = new CliCommandOutputOutcome("Exiting CLI workflow.");
        return Task.FromResult<CliCommandOutcome[]>([outcome]);
    }
}
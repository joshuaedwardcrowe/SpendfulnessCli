using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Instructions.Abstractions;

namespace Cli.Workflow.Commands;

public interface ICliWorkflowCommandProvider
{
    CliCommand GetCommand(CliInstruction instruction, List<CliCommandOutcome> outcomes);
}
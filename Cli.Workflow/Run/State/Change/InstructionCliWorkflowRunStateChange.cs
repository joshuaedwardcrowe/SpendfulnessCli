using Cli.Instructions.Abstractions;
using Cli.Workflow.Abstractions;

namespace Cli.Workflow.Run.State.Change;

public class InstructionCliWorkflowRunStateChange(
    TimeSpan at,
    ClIWorkflowRunStateStatus from,
    ClIWorkflowRunStateStatus to,
    CliInstruction instruction)
    : CliWorkflowRunStateChange(at, from, to)
{
    public readonly CliInstruction Instruction = instruction;
}
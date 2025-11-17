using Cli.Instructions.Abstractions;
using Cli.Workflow.Abstractions;

namespace Cli.Workflow;

public class InstructionCliWorkflowRunStateChange(
    TimeSpan at,
    ClIWorkflowRunStateType from,
    ClIWorkflowRunStateType to,
    CliInstruction instruction)
    : CliWorkflowRunStateChange(at, from, to)
{
    public readonly CliInstruction Instruction = instruction;
}
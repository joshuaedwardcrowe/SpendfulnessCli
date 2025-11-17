using Cli.Instructions.Abstractions;

namespace Cli.Instructions.Validators;

public interface ICliInstructionValidator
{
    bool IsValidInstruction(CliInstruction instruction);
}
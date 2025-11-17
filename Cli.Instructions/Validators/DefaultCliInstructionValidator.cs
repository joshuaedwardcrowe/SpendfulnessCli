using Cli.Instructions.Abstractions;

namespace Cli.Instructions.Validators;

public class DefaultCliInstructionValidator : ICliInstructionValidator
{
    public bool IsValidInstruction(CliInstruction instruction)
    {
        if (instruction.Prefix is null)
        {
            return false;
        }

        if (instruction.Name is null)
        {
            return false;
        }

        return true;
    }

}
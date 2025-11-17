namespace Cli.Instructions.Abstractions.Validators;

public interface ICliInstructionValidator
{
    bool IsValidInstruction(CliInstruction instruction);
}
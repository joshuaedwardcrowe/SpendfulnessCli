using Cli.Instructions.Abstractions;

namespace Cli.Commands.Abstractions;

// TODO: Rename to IUsableCliCommandGenerator?
public interface IGenericCommandGenerator
{
    ICliCommand Generate(CliInstruction instruction);
}
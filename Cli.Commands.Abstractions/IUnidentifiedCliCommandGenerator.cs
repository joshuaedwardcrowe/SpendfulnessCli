using Cli.Instructions.Abstractions;

namespace Cli.Commands.Abstractions;

public interface IUnidentifiedCliCommandGenerator
{
    CliCommand Generate(CliInstruction instruction);
}
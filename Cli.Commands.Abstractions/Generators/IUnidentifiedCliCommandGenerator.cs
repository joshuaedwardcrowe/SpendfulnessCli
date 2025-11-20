using Cli.Instructions.Abstractions;

namespace Cli.Commands.Abstractions.Generators;

public interface IUnidentifiedCliCommandGenerator
{
    CliCommand Generate(CliInstruction instruction);
}
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;

namespace Cli.Commands.Abstractions.Generators;

public interface IUnidentifiedCliCommandGenerator
{
    bool CanGenerate(CliInstruction instruction, List<CliCommandProperty> properties) => true;
    
    CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties);
}
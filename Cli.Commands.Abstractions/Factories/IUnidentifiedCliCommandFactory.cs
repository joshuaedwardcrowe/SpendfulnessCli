using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;

namespace Cli.Commands.Abstractions.Factories;

public interface IUnidentifiedCliCommandFactory
{
    bool CanGenerate(CliInstruction instruction, List<CliCommandProperty> properties) => true;
    
    CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties);
}
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;

namespace Cli.Commands.Abstractions.Factories;

public interface IUnidentifiedCliCommandFactory
{
    bool CanCreateWhen(CliInstruction instruction, List<CliCommandProperty> properties) => true;
    
    CliCommand Create(CliInstruction instruction, List<CliCommandProperty> properties);
}
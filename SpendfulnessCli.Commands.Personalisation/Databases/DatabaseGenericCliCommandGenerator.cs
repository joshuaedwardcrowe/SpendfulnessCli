using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Commands.Personalisation.Databases.Create;

namespace SpendfulnessCli.Commands.Personalisation.Databases;

public class DatabaseGenericCliCommandGenerator : ICliCommandGenerator<DatabaseCliCommand>
{
    public CliCommand Generate(CliInstruction instruction) =>
        instruction.SubInstructionName switch
        {
            DatabaseCliCommand.SubCommandNames.Create => new DatabaseCreateCliCommand(),
            _ => new DatabaseCliCommand()
        };
}
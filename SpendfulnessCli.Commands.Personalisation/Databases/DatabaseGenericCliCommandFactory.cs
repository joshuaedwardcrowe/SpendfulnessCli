using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Commands.Personalisation.Databases.Create;

namespace SpendfulnessCli.Commands.Personalisation.Databases;

public class DatabaseGenericCliCommandFactory : ICliCommandFactory<DatabaseCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
        => instruction.SubInstructionName switch
        {
            DatabaseCliCommand.SubCommandNames.Create => new DatabaseCreateCliCommand(),
            _ => new DatabaseCliCommand()
        };
}
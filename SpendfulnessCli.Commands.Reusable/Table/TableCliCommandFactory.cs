using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reusable.Table;

[FactoryFor(typeof(TableCliCommand))]
public class TableCliCommandGenerator : ICliCommandFactory<TableCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> properties) => properties.Count == 0;

    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties) => new TableCliCommand();
}
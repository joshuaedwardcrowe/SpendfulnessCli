using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reusable.Table;

[FactoryFor(typeof(TableCliCommand))]
public class TableCliCommandGenerator : ICliCommandFactory<TableCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandProperty> properties) => properties.Count == 0;

    public CliCommand Create(CliInstruction instruction, List<CliCommandProperty> properties) => new TableCliCommand();
}
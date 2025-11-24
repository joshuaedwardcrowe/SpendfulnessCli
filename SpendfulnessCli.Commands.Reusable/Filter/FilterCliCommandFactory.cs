using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reusable.Filter;

public class FilterCliCommandFactory : RootCliCommandFactory, ICliCommandFactory<FilterCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
        => new FilterCliCommand(string.Empty);
}
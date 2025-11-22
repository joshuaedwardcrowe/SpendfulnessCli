using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Categories;

public class CategoriesGenericCliCommandFactory : ICliCommandFactory<CategoriesCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
        => new CategoriesCliCommand();
}
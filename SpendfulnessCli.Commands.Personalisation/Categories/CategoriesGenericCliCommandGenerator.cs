using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Generators;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Categories;

public class CategoriesGenericCliCommandGenerator : ICliCommandGenerator<CategoriesCliCommand>
{
    public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
        => new CategoriesCliCommand();
}
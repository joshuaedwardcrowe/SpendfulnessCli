using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Generators;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Categories;

public class CategoriesGenericCliCommandGenerator : ICliCommandGenerator<CategoriesCliCommand>
{
    public CliCommand Generate(CliInstruction instruction)
    {
        return new CategoriesCliCommand();
    }
}
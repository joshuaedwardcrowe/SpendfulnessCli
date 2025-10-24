using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;

namespace Cli.Spendfulness.Commands.Personalisation.Categories;

public class CategoriesGenericCommandGenerator : ICommandGenerator<CategoriesCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
    {
        return new CategoriesCommand();
    }
}
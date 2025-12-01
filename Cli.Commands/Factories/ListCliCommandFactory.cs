using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Commands.Factories;

public abstract class ListCliCommandFactory
{
    protected static (ValuedCliInstructionArgument<int>? pageNumberArgument, ValuedCliInstructionArgument<int>? pageSizeArgument) GetPagingArguments(CliInstruction instruction)
    {
        var pageSizeArgument = instruction
            .Arguments
            .OfType<int>(ListCliCommand.ArgumentNames.PageSize);

        var pageNumberArgument = instruction
            .Arguments
            .OfType<int>(ListCliCommand.ArgumentNames.PageNumber);

        return (pageNumberArgument, pageSizeArgument);
    }
}
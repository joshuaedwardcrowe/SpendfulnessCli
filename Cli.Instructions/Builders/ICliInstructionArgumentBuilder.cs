using Cli.Instructions.Abstractions;

namespace Cli.Instructions.Builders;

public interface ICliInstructionArgumentBuilder
{
    bool For(string? argumentValue);

    ConsoleInstructionArgument Create(string argumentName, string? argumentValue);
}

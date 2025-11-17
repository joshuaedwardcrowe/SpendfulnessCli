using Cli.Instructions.Abstractions;

namespace Cli.Instructions.Builders;

public interface ICliInstructionArgumentBuilder
{
    bool For(string? argumentValue);

    CliInstructionArgument Create(string argumentName, string? argumentValue);
}

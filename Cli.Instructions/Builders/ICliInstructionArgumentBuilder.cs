using Cli.Instructions.Abstractions;

namespace Cli.Instructions.Builders;

internal interface ICliInstructionArgumentBuilder
{
    bool For(string? argumentValue);

    CliInstructionArgument Create(string argumentName, string? argumentValue);
}

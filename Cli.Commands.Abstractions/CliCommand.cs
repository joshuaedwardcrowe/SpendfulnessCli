using Cli.Commands.Abstractions.Extensions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Instructions.Abstractions;
using MediatR;

namespace Cli.Commands.Abstractions;

/// <summary>
/// A command that can be executed via the CLI.
/// For example, "List all transactions for payee X".
/// </summary>
public record CliCommand : IRequest<CliCommandOutcome[]>
{
    public string GetSpecificCommandName()
        => GetType().Name.ReplaceCommandSuffix();

    public string GetInstructionName()
        => GetType().Name.ReplaceCommandSuffix()
            .ToLowerSplitString(CliInstructionConstants.DefaultCommandNameSeparator);

    public static string StripInstructionName(string commandName)
        => commandName
            .ReplaceCommandSuffix()
            .ToLowerSplitString(CliInstructionConstants.DefaultCommandNameSeparator);
}
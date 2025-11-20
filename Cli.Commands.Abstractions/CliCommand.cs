using Cli.Commands.Abstractions.Outcomes;
using MediatR;

namespace Cli.Commands.Abstractions;

/// <summary>
/// A command that can be executed via the CLI.
/// For example, "List all transactions for payee X".
/// </summary>
public record CliCommand : IRequest<CliCommandOutcome[]>
{
    public string GetCommandName()
    {
        var commandSuffix = nameof(CliCommand);
        return GetType().Name.Replace(commandSuffix, string.Empty);
    }
}
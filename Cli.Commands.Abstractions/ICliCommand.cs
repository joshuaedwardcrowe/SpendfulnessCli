using Cli.Commands.Abstractions.Outcomes;
using MediatR;

namespace Cli.Commands.Abstractions;

/// <summary>
/// A command that can be executed via the CLI.
/// For example, "List all transactions for payee X".
/// </summary>
public record CliCommand : IRequest<CliCommandOutcome>
{
    /// <summary>
    /// Get the name of the command.
    /// (without the suffix)
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCliCommand"></typeparam>
    /// <returns></returns>
    public string GetCommandName()
    {
        var commandSuffix = nameof(CliCommand);
        return GetType().Name.Replace(commandSuffix, string.Empty);
    }
}
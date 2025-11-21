using Cli.Commands.Abstractions.Outcomes;
using MediatR;

namespace Cli.Commands.Abstractions;

/// <summary>
/// A command that can be executed via the CLI.
/// For example, "List all transactions for payee X".
/// </summary>
public record CliCommand : IRequest<CliCommandOutcome[]>
{
    private const string CommandSuffix = nameof(CliCommand);
    
    public string GetSpecificCommandName()
        => GetType().Name.Replace(CommandSuffix, string.Empty);

    public static string StripSpecificCommandName(string commandName)
        => commandName.Replace(CommandSuffix, string.Empty);
}
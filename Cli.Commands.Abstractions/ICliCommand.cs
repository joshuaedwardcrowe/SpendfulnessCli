using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Properties;
using MediatR;

namespace Cli.Commands.Abstractions;

/// <summary>
/// A command that can be executed via the CLI.
/// For example, "List all transactions for payee X".
/// </summary>
public abstract record CliCommand : IRequest<CliCommandOutcome>
{
    public Dictionary<string, CliCommandProperty> Properties { get; set; }
    public bool IsContinuous { get; set; }
    
    protected CliCommand(bool isContinuous = false)
    {
        Properties = new Dictionary<string, CliCommandProperty>();
        IsContinuous = isContinuous;
    }
    
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
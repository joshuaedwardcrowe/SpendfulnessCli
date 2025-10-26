using Cli.Commands.Abstractions.Outcomes;
using MediatR;

namespace Cli.Commands.Abstractions;

public interface ICliCommand : IRequest<CliCommandOutcome>
{
}

public static class CliCommand
{
    public static string GetName<TCliCommand>(this TCliCommand command) where TCliCommand : ICliCommand
    {
        var commandSuffix = nameof(CliCommand);
        var commandType = typeof(TCliCommand);
        return commandType.Name.Replace(commandSuffix, string.Empty);
    }
}
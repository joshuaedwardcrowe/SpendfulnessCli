namespace Cli.Commands.Abstractions;

public static class CliCommandExtensions
{
    public static string GetCommandName<TCliCommand>(this TCliCommand command) where TCliCommand : CliCommand
    {
        var commandSuffix = nameof(CliCommand);
        var commandType = typeof(TCliCommand);
        return commandType.Name.Replace(commandSuffix, string.Empty);
    }

    public static string GetCommandName(this Type commandType)
    {
        var commandSuffix = nameof(CliCommand);
        return commandType.Name.Replace(commandSuffix, string.Empty);
    }
}
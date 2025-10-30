namespace Cli.Commands.Abstractions;

public static class CliCommandExtensions
{
    public static string GetName<TCliCommand>(this TCliCommand command) where TCliCommand : CliCommand
    {
        var commandSuffix = nameof(CliCommand);
        var commandType = typeof(TCliCommand);
        return commandType.Name.Replace(commandSuffix, string.Empty);
    }

    public static string GetNameFromType<TCliCommandType>(this TCliCommandType commandType) where TCliCommandType : Type
    {
        var commandSuffix = nameof(CliCommand);
        return commandType.Name.Replace(commandSuffix, string.Empty);
    }
}
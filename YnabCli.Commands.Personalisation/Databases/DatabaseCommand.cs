namespace YnabCli.Commands.Personalisation.Databases;

public class DatabaseCommand : ICommand
{
    public const string CommandName = "database";
    public const string ShorthandCommandName = "db";
    public static class SubCommandNames
    {
        public const string Create = "create";
    }
}
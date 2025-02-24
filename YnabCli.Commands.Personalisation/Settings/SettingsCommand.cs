namespace YnabCli.Commands.Personalisation.Settings;

public class SettingsCommand : ICommand
{
    public const string CommandName = "settings";
    public const string ShorthandCommandName = "s";

    public static class SubCommandNames
    {
        public const string Create = "create";
        public const string View = "view";
    }
}
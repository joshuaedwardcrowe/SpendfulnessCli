namespace YnabCli.Commands.Personalisation.Users;

public class UserCommand : ICommand
{
    public const string CommandName = "user";
    public const string ShorthandCommandName = "u";

    public static class SubCommandNames
    {
        public const string Create = "create";
        public const string Switch = "switch";
        public const string Active = "active";
    }
}
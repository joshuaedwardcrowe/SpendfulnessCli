namespace YnabCli.Commands.Database.Users.Switch;

public class UserSwitchCommand : ICommand
{
    public static class ArugmentNames
    {
        public const string UserName = "user-name";
    }
    
    public string? UserName { get; set; }
}
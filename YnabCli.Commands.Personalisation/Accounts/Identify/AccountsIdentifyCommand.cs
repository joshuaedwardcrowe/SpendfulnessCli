namespace YnabCli.Commands.Personalisation.Accounts.Identify;

public class AccountsIdentifyCommand : ICommand
{
    public static class ArgumentNames
    {
        public const string Name = "name";
        public const string Type = "type";
    }
    
    public string YnabAccountName { get; set; }
    public string AccountAccountTypeName { get; set; }
}
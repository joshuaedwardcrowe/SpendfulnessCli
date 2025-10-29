using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Users.Create;

public class UserCreateCliCommand : ICliCommand
{
    public static class ArugmentNames
    {
        public const string UserName = "user-name";
    }
    
    public required string UserName { get; set; }
}
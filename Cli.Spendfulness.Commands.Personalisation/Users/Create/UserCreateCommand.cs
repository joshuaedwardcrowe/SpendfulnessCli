using Cli.Commands.Abstractions;

namespace Cli.Spendfulness.Commands.Personalisation.Users.Create;

public class UserCreateCommand : ICommand
{
    public static class ArugmentNames
    {
        public const string UserName = "user-name";
    }
    
    public required string UserName { get; set; }
}
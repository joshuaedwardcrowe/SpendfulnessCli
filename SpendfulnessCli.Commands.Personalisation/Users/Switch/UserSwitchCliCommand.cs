using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Users.Switch;

public record UserSwitchCliCommand : CliCommand
{
    public static class ArugmentNames
    {
        public const string UserName = "user-name";
    }
    
    public string? UserName { get; set; }
}
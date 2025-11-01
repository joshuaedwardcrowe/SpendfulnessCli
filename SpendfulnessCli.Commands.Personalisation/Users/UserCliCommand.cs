using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Users;

public record UserCliCommand : CliCommand
{
    public static class SubCommandNames
    {
        public const string Create = "create";
        public const string Switch = "switch";
        public const string Active = "active";
    }
}
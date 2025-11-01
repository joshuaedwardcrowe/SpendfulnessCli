using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Databases;

public record DatabaseCliCommand : CliCommand
{
    public static class SubCommandNames
    {
        public const string Create = "create";
    }
}
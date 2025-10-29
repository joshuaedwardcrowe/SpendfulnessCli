using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Databases;

public class DatabaseCliCommand : ICliCommand
{
    public static class SubCommandNames
    {
        public const string Create = "create";
    }
}
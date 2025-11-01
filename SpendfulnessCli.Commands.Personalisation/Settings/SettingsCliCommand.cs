using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Settings;

public record SettingsCliCommand : CliCommand
{
    public static class SubCommandNames
    {
        public const string Create = "create";
        public const string View = "view";
    }
}
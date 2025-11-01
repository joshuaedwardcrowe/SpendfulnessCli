using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Accounts;

public record AccountsCliCommand : CliCommand
{
    public static class SubCommandNames
    {
        public const string Identify = "identify";
        public const string ReconcileRewards = "reconcile-rewards";
    }
}
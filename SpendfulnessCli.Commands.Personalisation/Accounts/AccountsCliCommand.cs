using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Accounts;

public class AccountsCliCommand : ICliCommand
{
    public static class SubCommandNames
    {
        public const string Identify = "identify";
        public const string ReconcileRewards = "reconcile-rewards";
    }
}
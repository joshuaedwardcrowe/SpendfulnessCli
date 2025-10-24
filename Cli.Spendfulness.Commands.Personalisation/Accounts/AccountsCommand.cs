using Cli.Commands.Abstractions;

namespace Cli.Spendfulness.Commands.Personalisation.Accounts;

public class AccountsCommand : ICommand
{
    public static class SubCommandNames
    {
        public const string Identify = "identify";
        public const string Create = "create";
    }
}
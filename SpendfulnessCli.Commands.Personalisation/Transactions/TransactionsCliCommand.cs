using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Transactions;

public class TransactionsCliCommand : ICliCommand
{
    public static class SubCommandNames
    {
        public const string List = "list";
    }
}
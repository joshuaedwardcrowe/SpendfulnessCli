using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Transactions;

public record TransactionsCliCommand : CliCommand
{
    public static class SubCommandNames
    {
        public const string List = "list";
    }
}
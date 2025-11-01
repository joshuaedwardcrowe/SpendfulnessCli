using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Transactions.List;

public record TransactionsListCliCommand : CliCommand
{
    public static class ArgumentNames
    {
        public const string PayeeName = "payeeName";
    }
    
    public string? PayeeName { get;  set; }
}
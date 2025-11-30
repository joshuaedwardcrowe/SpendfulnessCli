using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.Transactions.List;

public record ListTransactionCliCommand : CliCommand
{
    public static class ArgumentNames
    {
        public const string PayeeName = "payeeName";
    }
    
    public string? PayeeName { get;  set; }
}
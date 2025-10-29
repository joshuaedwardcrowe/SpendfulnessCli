using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Transactions.List;

public class TransactionsListCliCommand : ICliCommand
{
    public static class ArgumentNames
    {
        public const string PayeeName = "payeeName";
    }
    
    public string? PayeeName { get;  set; }
}
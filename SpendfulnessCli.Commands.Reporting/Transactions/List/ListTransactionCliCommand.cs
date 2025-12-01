using Cli.Commands;

namespace SpendfulnessCli.Commands.Reporting.Transactions.List;

public record ListTransactionCliCommand : ListCliCommand
{
    public new static class ArgumentNames
    {
        public const string PayeeName = "payeeName";
    }
    
    public string? PayeeName { get;  set; }
    
    public ListTransactionCliCommand(int? pageNumber, int? pageSize) : base(pageNumber, pageSize)
    {
    }
}
namespace YnabCli.Commands.Personalisation.Transactions.List;

public class TransactionsListCommand : ICommand
{
    public static class ArgumentNames
    {
        public const string PayeeName = "payeeName";
    }
    
    public string? PayeeName { get;  set; }
}
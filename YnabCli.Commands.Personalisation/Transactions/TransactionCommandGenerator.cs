using YnabCli.Commands.Generators;
using YnabCli.Commands.Personalisation.Transactions.List;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Personalisation.Transactions;

public class TransactionCommandGenerator : ICommandGenerator<TransactionsCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
        => subCommandName switch
        {
            TransactionsCommand.SubCommandNames.List => CreateListCommand(arguments),
            _ => new TransactionsListCommand(),
        };

    private TransactionsListCommand CreateListCommand(List<InstructionArgument> arguments)
    {
        var payeeNameArgument = arguments.OfType<string>(TransactionsListCommand.ArgumentNames.PayeeName);

        return new TransactionsListCommand
        {
            PayeeName = payeeNameArgument?.ArgumentValue
        };
    }
}
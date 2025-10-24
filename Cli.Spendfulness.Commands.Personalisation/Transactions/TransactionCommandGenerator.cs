using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Spendfulness.Commands.Personalisation.Transactions.List;

namespace Cli.Spendfulness.Commands.Personalisation.Transactions;

public class TransactionCommandGenerator : ICommandGenerator<TransactionsCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
        => subCommandName switch
        {
            TransactionsCommand.SubCommandNames.List => CreateListCommand(arguments),
            _ => new TransactionsListCommand(),
        };

    private TransactionsListCommand CreateListCommand(List<ConsoleInstructionArgument> arguments)
    {
        var payeeNameArgument = arguments.OfType<string>(TransactionsListCommand.ArgumentNames.PayeeName);

        return new TransactionsListCommand
        {
            PayeeName = payeeNameArgument?.ArgumentValue
        };
    }
}
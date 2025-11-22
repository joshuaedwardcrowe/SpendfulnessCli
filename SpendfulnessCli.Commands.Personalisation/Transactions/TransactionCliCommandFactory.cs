using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using SpendfulnessCli.Commands.Personalisation.Transactions.List;

namespace SpendfulnessCli.Commands.Personalisation.Transactions;

public class TransactionCliCommandFactory : ICliCommandFactory<TransactionsCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandProperty> properties)
        => instruction.SubInstructionName switch
        {
            TransactionsCliCommand.SubCommandNames.List => CreateListCommand(instruction.Arguments),
            _ => new TransactionsListCliCommand(),
        };
    
    private TransactionsListCliCommand CreateListCommand(List<CliInstructionArgument> arguments)
    {
        var payeeNameArgument = arguments.OfType<string>(TransactionsListCliCommand.ArgumentNames.PayeeName);

        return new TransactionsListCliCommand
        {
            PayeeName = payeeNameArgument?.ArgumentValue
        };
    }
}
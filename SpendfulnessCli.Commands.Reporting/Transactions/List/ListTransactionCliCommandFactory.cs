using Cli.Commands;
using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Factories;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Reporting.Transactions.List;

public class ListTransactionCliCommandFactory : ListCliCommandFactory, ICliCommandFactory<TransactionsCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
        => instruction.SubInstructionName == TransactionsCliCommand.SubCommandNames.List;

    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var (pageSize, pageNumber) = GetPaging(instruction, artefacts);
        
        // TODO: Move to filter command.
        var payeeNameArgument = instruction
            .Arguments
            .OfType<string>(ListTransactionCliCommand.ArgumentNames.PayeeName);

        return new ListTransactionCliCommand(pageNumber, pageSize)
        {
            PayeeName = payeeNameArgument?.ArgumentValue
        };
    }
}
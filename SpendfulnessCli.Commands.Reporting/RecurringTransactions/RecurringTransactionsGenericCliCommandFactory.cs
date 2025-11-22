using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Reporting.RecurringTransactions;

public class RecurringTransactionsGenericCliCommandFactory : ICliCommandFactory<RecurringTransactionsCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
    {
        var fromArgument = instruction
            .Arguments
            .OfType<DateOnly>(RecurringTransactionsCliCommand.ArgumentNames.From);
        
        var toArgument = instruction
            .Arguments
            .OfType<DateOnly>(RecurringTransactionsCliCommand.ArgumentNames.To);
        
        var payeeNameArgument = instruction
            .Arguments
            .OfType<string>(RecurringTransactionsCliCommand.ArgumentNames.PayeeName);
        
        var minimumOccurrencesArgument = instruction
            .Arguments
            .OfType<int>(RecurringTransactionsCliCommand.ArgumentNames.MinimumOccurrences);

        return new RecurringTransactionsCliCommand
        {
            From = fromArgument?.ArgumentValue,
            To = toArgument?.ArgumentValue,
            PayeeName = payeeNameArgument?.ArgumentValue,
            MinimumOccurrences = minimumOccurrencesArgument?.ArgumentValue
        };
    }
}
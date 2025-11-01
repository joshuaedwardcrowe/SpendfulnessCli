using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Reporting.RecurringTransactions;

public class RecurringTransactionsGenericCliCommandGenerator : ICliCommandGenerator<RecurringTransactionsCliCommand>
{
    public CliCommand Generate(CliInstruction instruction)
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
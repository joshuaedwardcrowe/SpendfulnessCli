using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Spendfulness.Commands.Personalisation.Samples.Matches;

namespace Cli.Spendfulness.Commands.Personalisation.Samples;

public class SamplesCommandGenerator : ICommandGenerator<SamplesCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
        => subCommandName switch
        {
            SamplesCommand.SubCommandNames.Matches => GetMatchesCommand(arguments),
            _ => new SamplesCommand()
        };

    private SamplesMatchesCommand GetMatchesCommand(List<ConsoleInstructionArgument> arguments)
    {
        var trnasactionIdArgument = arguments
            .OfRequiredStringFrom<Guid, string>(SamplesMatchesCommand.ArgumentNames.TransactionId);

        return new SamplesMatchesCommand
        {
            TransactionId = trnasactionIdArgument.ArgumentValue
        };
    }
}
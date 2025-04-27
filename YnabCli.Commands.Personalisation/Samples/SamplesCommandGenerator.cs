using YnabCli.Commands.Generators;
using YnabCli.Commands.Personalisation.Samples.Matches;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Personalisation.Samples;

public class SamplesCommandGenerator : ICommandGenerator<SamplesCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
        => subCommandName switch
        {
            SamplesCommand.SubCommandNames.Matches => GetMatchesCommand(arguments),
            _ => new SamplesCommand()
        };

    private SamplesMatchesCommand GetMatchesCommand(List<InstructionArgument> arguments)
    {
        var trnasactionIdArgument = arguments
            .OfRequiredStringFrom<Guid, string>(SamplesMatchesCommand.ArgumentNames.TransactionId);

        return new SamplesMatchesCommand
        {
            TransactionId = trnasactionIdArgument.ArgumentValue
        };
    }
}
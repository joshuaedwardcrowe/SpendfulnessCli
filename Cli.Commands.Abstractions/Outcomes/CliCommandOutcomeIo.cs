
using Cli.Commands.Abstractions.Exceptions;
using Cli.Commands.Abstractions.Io;

namespace Cli.Commands.Abstractions.Outcomes;

public class CliCommandOutcomeIo : CliIo
{
    public void Say(CliCommandOutcome outcome)
    {
        switch (outcome)
        {
            case CliCommandTableOutcome tableOutcome:
                Say(tableOutcome);
                break;
            case CliCommandOutputOutcome outputOutcome:
                Say(outputOutcome);
                break;
            case CliCommandNotFoundOutcome nothingOutcome:
                Say(nothingOutcome);
                break;
            case CliCommandExceptionOutcome exceptionOutcome:
                Say(exceptionOutcome);
                break;
            case CliCommandNothingOutcome:
                break;
            default:
                throw new UnknownOutcomeException(
                    $"{outcome.GetType().Name} outcomes not supported");
        }
    }
    public void Say(CliCommandTableOutcome tableOutcome)
        => Say(tableOutcome.Table.ToString());
    
    public void Say(CliCommandOutputOutcome outputOutcome)
        => Say(outputOutcome.Output);
    
    public void Say(CliCommandNothingOutcome cliCommandNothingOutcome)
        => Say("Command not found.");
}
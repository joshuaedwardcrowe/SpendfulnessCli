using Cli.Outcomes;

namespace Cli;

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
            case CliCommandNothingOutcome nothingOutcome:
                Say(nothingOutcome);
                break;
            default:
                throw new UnknownCliCommandOutcomeException(
                    $"{outcome.Kind} outcomes not supported");
        }
    }
    public void Say(CliCommandTableOutcome tableOutcome)
        => Say(tableOutcome.Table.ToString());
    
    public void Say(CliCommandOutputOutcome outputOutcome)
        => Say(outputOutcome.Output);
    
    public void Say(CliCommandNothingOutcome nothingOutcome)
        => Say($"Command resulted in nothing. Last state: {nothingOutcome}");
}
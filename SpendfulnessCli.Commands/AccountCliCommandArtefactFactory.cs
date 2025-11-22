using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Outcomes;

namespace SpendfulnessCli.Commands;

public class AccountCliCommandArtefactFactory : ICliCommandArtefactFactory
{
    public bool CanCreateWhen(CliCommandOutcome outcome)
        => outcome is AccountCliCommandOutcome;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        var accountOutcome = (AccountCliCommandOutcome)outcome;
        return new AccountCliCommandArtefact(accountOutcome.Account);
    }
}
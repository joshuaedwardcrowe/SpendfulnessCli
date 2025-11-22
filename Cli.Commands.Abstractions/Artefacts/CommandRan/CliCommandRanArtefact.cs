namespace Cli.Commands.Abstractions.Artefacts.CommandRan;

public class CliCommandRanArtefact(CliCommand ranCommand) : CliCommandArtefact
{
    public CliCommand RanCommand { get; } = ranCommand;
}
namespace Cli.Commands.Abstractions.Artefacts.CommandRan;

public class RanCliCommandArtefact(CliCommand ranCommand) : CliCommandArtefact
{
    public CliCommand RanCommand { get; } = ranCommand;
}
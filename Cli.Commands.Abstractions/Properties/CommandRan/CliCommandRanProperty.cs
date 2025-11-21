namespace Cli.Commands.Abstractions.Properties.CommandRan;

public class CliCommandRanProperty(CliCommand ranCommand) : CliCommandProperty
{
    public CliCommand RanCommand { get; } = ranCommand;
}
namespace Cli.Abstractions;

public abstract class CliAggregator<TAggregation>
{
    public abstract TAggregation Aggregate();
}
using Cli.Abstractions;
using Cli.Commands.Abstractions.Properties.CommandRan;

namespace Cli.Commands.Abstractions.Properties;

public static class CliCommandPropertyExtensions
{
    public static bool AnyCommandRan<TCliCommand>(this List<CliCommandProperty> properties) where TCliCommand : CliCommand
        => properties
            .OfType<CliCommandRanProperty>()
            .Any(c => c.RanCommand is TCliCommand);
    
    public static List<ListAggregatorCliCommandProperty<TAggregate>> OfListAggregatorType<TAggregate>(
        this List<CliCommandProperty> properties)
        => properties.OfType<ListAggregatorCliCommandProperty<TAggregate>>().ToList();

    public static CliListAggregator<TAggregate>? GetListAggregator<TAggregate>(
        this List<CliCommandProperty> properties)
        => properties
            .OfListAggregatorType<TAggregate>()
            .FirstOrDefault()
            ?.Value;
}
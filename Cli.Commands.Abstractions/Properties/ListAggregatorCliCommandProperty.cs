using Cli.Abstractions;

namespace Cli.Commands.Abstractions.Properties;

public class ListAggregatorCliCommandProperty<TAggregate>(CliListAggregator<TAggregate> value)
    : ValuedCliCommandProperty<CliListAggregator<TAggregate>>(value)
{
    
}
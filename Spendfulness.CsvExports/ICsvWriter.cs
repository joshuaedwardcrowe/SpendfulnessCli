using SpendfulnessCli.Aggregation.Aggregator;

namespace Spendfulness.CsvExports;

public interface ICsvWriter<TCsvRow> where TCsvRow : class, new()
{
    public Task Write(YnabListAggregator<TCsvRow> aggregator);
}
using SpendfulnessCli.Aggregation.Aggregates;
using SpendfulnessCli.CliTables.ViewModels;

namespace SpendfulnessCli.CliTables.ViewModelBuilders;

public class TransactionPotentialSplitCliTableBuilder : CliTableBuilder<PotentialTransactionSplitAggregate>
{
    protected override List<string> BuildColumnNames(IEnumerable<PotentialTransactionSplitAggregate> aggregates)
        => PotentialTrasactionSplitCliTable.GetColumnNames();

    protected override List<List<object>> BuildRows(IEnumerable<PotentialTransactionSplitAggregate> aggregates)
        => aggregates
            .Select(BuildIndividualRow)
            .Select(r => r.ToList())
            .ToList();
    
    private IEnumerable<object> BuildIndividualRow(PotentialTransactionSplitAggregate aggregate)
    {
        yield return aggregate.OriginalTransactionId;
        yield return aggregate.PotentialSplitPayeeName;
        yield return aggregate.PotentialSplitCategoryName;
        yield return aggregate.PotentialSplitMemo;
        yield return aggregate.PotentialSplitAmount;
    }
}
using Cli.Abstractions;
using SpendfulnessCli.Abstractions.Splittables;
using SpendfulnessCli.Aggregation.Aggregates;
using Ynab;

namespace SpendfulnessCli.Aggregation.Aggregator.ListAggregators;

public class PotentialTransactionSplitListAggregator(IEnumerable<Transaction> transactions)
    : YnabListAggregator<PotentialTransactionSplitAggregate>(transactions)
{
    protected override IEnumerable<PotentialTransactionSplitAggregate> GenerateAggregate()
    {
        var splitTransactions = Transactions
            .Where(transaction => transaction.SplitTransactions.Any() &&
                                  transaction.SplitTransactions.All(st => st.Memo != null))
            .ToList();
            
        var currentSplittableTransactions = Transactions
            .OfType<SplittableTransaction>()
            .ToList();

        // TODO: Find splittable transactions and what they can possibly be split into.
        var potentialSplitConversions = new Dictionary<SplittableTransaction, Dictionary<string, SplitTransactions>>();
        foreach (var currentSplittableTransaction in currentSplittableTransactions)
        {
            var memoNotes = currentSplittableTransaction.MemoNotes;

            var splitTransactionsForPayee = splitTransactions
                .Where(s => s.PayeeId == currentSplittableTransaction.PayeeId && s.Memo != null)
                .ToList();

            if (!splitTransactionsForPayee.Any())
            {
                continue;
            }

            var matches = new Dictionary<string, SplitTransactions>();
            foreach (var memoNote in memoNotes)
            {
                var l = splitTransactionsForPayee
                    .SelectMany(splitTransaction => splitTransaction.SplitTransactions)
                    .FirstOrDefault(q => q.Memo!.IsSimilarTo(memoNote));

                if (l == null)
                {
                    continue;
                }

                matches.Add(memoNote, l);
            }

            if (matches.Any())
            {
                potentialSplitConversions.Add(currentSplittableTransaction, matches);
            }
        }

        // TODO: Can convert to .SelectMany.
        var aggs = new List<PotentialTransactionSplitAggregate>();
        foreach (var (splittableTransaction, matches) in potentialSplitConversions)
        {
            foreach (var (_, potentialCloneTransaction) in matches)
            {
                var cloneHasPayeeeName = !string.IsNullOrWhiteSpace(potentialCloneTransaction.PayeeName);
                var potentialSplitPayeeName = cloneHasPayeeeName
                    ? potentialCloneTransaction.PayeeName
                    : splittableTransaction.PayeeName;
                
                var agg = new PotentialTransactionSplitAggregate(
                    splittableTransaction.Id,
                    splittableTransaction.PayeeName,
                    splittableTransaction.CategoryName,
                    splittableTransaction.Memo!,
                    splittableTransaction.Amount,
                    potentialSplitPayeeName,
                    potentialCloneTransaction.CategoryName,
                    potentialCloneTransaction.Memo!,
                    potentialCloneTransaction.Amount);
                
                aggs.Add(agg);
            }
        }
        
        return aggs;
    }
}
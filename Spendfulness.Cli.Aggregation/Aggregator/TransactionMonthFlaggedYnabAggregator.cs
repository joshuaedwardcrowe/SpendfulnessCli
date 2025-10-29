using Spendfulness.Cli.Aggregation.Aggregates;
using Spendfulness.Cli.Aggregation.Aggregator.ListAggregators;
using Spendfulness.Cli.Aggregation.Calculators;
using Ynab;
using Ynab.Collections;
using Ynab.Extensions;

namespace Spendfulness.Cli.Aggregation.Aggregator;

public class TransactionMonthFlaggedYnabAggregator(IEnumerable<CategoryGroup> categoryGroups, IEnumerable<Transaction> transactions)
    : ListYnabAggregator<TransactionMonthFlaggedAggregate>(categoryGroups, transactions)
{
    protected override IEnumerable<TransactionMonthFlaggedAggregate> ListAggregate()
    {
        var spendingCategoryIds = CategoryGroups
            .FilterToSpendingCategories()
            .SelectMany(categoryGroup => categoryGroup.GetCategoryIds());

        var transactionGroups = Transactions
            .FilterToCategories(spendingCategoryIds)
            .GroupByMonth()
            .GroupByFlags()
            .ToList();

        var firstAggregate = EvaluateFirstGroup(transactionGroups);
        var remainingAggregates = EvaluateRemainingGroups(transactionGroups);
        
        return new List<TransactionMonthFlaggedAggregate> { firstAggregate }.Concat(remainingAggregates);
    }

    private TransactionMonthFlaggedAggregate EvaluateFirstGroup(List<TransactionsByFlagsByMonth> transactionGroups)
    {
        var firstGroup = transactionGroups.First();
        if (firstGroup is null)
        {
            // This is genuinely exceptional circumstances.
            throw new InvalidOperationException("No first group found");
        }

        var amountAggregates = EvaluateFirstSubGroups(firstGroup.TransactionsByFlags);

        return new TransactionMonthFlaggedAggregate(firstGroup.Month, amountAggregates.ToList());
    }
    
    private IEnumerable<TransactionMonthFlaggedAmountAggregate> EvaluateFirstSubGroups(List<TransactionsByFlag> transactionsSubGroups)
    {
        foreach (var transactionsSubGroup in transactionsSubGroups)
        {
            var amount = transactionsSubGroup.Transactions.Sum(transaction => transaction.Amount);
            yield return new TransactionMonthFlaggedAmountAggregate(transactionsSubGroup.Flag, amount, 0);
        }
    }

    private IEnumerable<TransactionMonthFlaggedAggregate> EvaluateRemainingGroups(List<TransactionsByFlagsByMonth> transactionGroups)
    {
        for (var i = 1; i < transactionGroups.Count; i++)
        {
            var priorMonthTransactionGroup = transactionGroups.ElementAt(i - 1);
            var currentMonthTransactionGroup = transactionGroups.ElementAt(i);
            
            var currentSomethings = EvaluateSomethings(
                priorMonthTransactionGroup.TransactionsByFlags,
                currentMonthTransactionGroup.TransactionsByFlags);
            
            yield return new TransactionMonthFlaggedAggregate(
                currentMonthTransactionGroup.Month, 
                currentSomethings.ToList());
        }
    }

    private IEnumerable<TransactionMonthFlaggedAmountAggregate> EvaluateSomethings(
        IEnumerable<TransactionsByFlag> priorMonthTransactionsSubGroups,
        IEnumerable<TransactionsByFlag> currentMonthTransactionsSubGroups)
    {
        var priorMonthTransactionsSubGroupsIndexed = priorMonthTransactionsSubGroups.ToList();
        
        // For each flag, compare prior and current mont
        foreach (var currentSubGroup in currentMonthTransactionsSubGroups)
        {
            var currentAmount = currentSubGroup.Transactions.Sum(x => x.Amount);
            
            var priorSubGroup = priorMonthTransactionsSubGroupsIndexed
                .FirstOrDefault(t => t.Flag == currentSubGroup.Flag);

            if (priorSubGroup is null)
            {
                yield return new TransactionMonthFlaggedAmountAggregate(
                    currentSubGroup.Flag,
                    currentAmount, 
                    0);
                
                continue;
            }
                
            var priorAmount = priorSubGroup.Transactions.Sum(x => x.Amount);

            var percentageChange = PercentageCalculator.CalculateChange(
                priorAmount,
                currentAmount);

            yield return new TransactionMonthFlaggedAmountAggregate(
                currentSubGroup.Flag,
                currentAmount,
                percentageChange);
        }
    }
}
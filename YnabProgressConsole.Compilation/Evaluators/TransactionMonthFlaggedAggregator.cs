using Ynab;
using Ynab.Collections;
using Ynab.Extensions;
using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Calculators;

namespace YnabProgressConsole.Compilation.Evaluators;

public class TransactionMonthFlaggedAggregator(IEnumerable<CategoryGroup> categoryGroups, IEnumerable<Transaction> transactions)
    : Aggregator<IEnumerable<TransactionMonthFlaggedAggregate>>(categoryGroups, transactions)
{
    public override IEnumerable<TransactionMonthFlaggedAggregate> Evaluate()
    {
        var spendingCategoryIds = CategoryGroups
            .FilterToSpendingCategories()
            .SelectMany(categoryGroup => categoryGroup.GetCategoryIds());

        var transactionGroups = Transactions
            .FilterByCategories(spendingCategoryIds)
            // .FilterByYear(2024)
            // .FilterFromMonth(2, 2024)
            // .FilterUpToMonth(5, 2024)
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
            throw new Exception("Could not do this");
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
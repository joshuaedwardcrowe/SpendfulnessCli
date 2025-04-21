using Ynab;
using Ynab.Collections;
using Ynab.Responses.Transactions;
using Ynab.Sanitisers;
using YnabCli.Aggregation.Extensions;
using YnabCli.ViewModels.Extensions;

namespace YnabCli.ViewModels.Tests.Extensions;

[TestFixture]
public class TransactionByMemoOccurrenceByPayeeNameExtensionsTests
{
    [Test]
    public void GivenDeeplyGroupedTransactions_WhenAggregate_ReturnsTransactionMemoOccurrenceAggregate()
    {
        var response = GetResponse();
        
        var transactions = GetCollection(response);
        
        var aggregate = transactions
            .AggregateMemoOccurrences()
            .First();
        
        var sanitisedAmount = MilliunitSanitiser.Calculate(response.Amount);
        
        Assert.That(aggregate.PayeeName, Is.EqualTo(response.PayeeName));
        Assert.That(aggregate.Memo, Is.EqualTo(response.Memo));
        Assert.That(aggregate.MemoOccurrence, Is.EqualTo(1));
        Assert.That(aggregate.AverageAmount, Is.EqualTo(sanitisedAmount));
    }

    private static TransactionResponse GetResponse()
        => new()
        {
            PayeeName = "payeeName",
            Memo = "memo",
            Amount = 2500,
            CategoryName = "categoryName"
        };

    private static List<TransactionsByMemoOccurrenceByPayeeName> GetCollection(TransactionResponse subTransactionResponse)
    {
        var transaction = new Transaction(subTransactionResponse);
        
        var transactionsByMemoOccurrences = new List<TransactionsByMemoOccurrence>
        {
            new()
            {
                Memo = transaction.Memo,
                MemoOccurence = 1,
                Transactions = [transaction]
            },
        };

        var transactionsByMemoOccurrenceByPayeeName = new TransactionsByMemoOccurrenceByPayeeName(
            transaction.PayeeName,
            transactionsByMemoOccurrences);
        
        
        return new List<TransactionsByMemoOccurrenceByPayeeName>
        {
            transactionsByMemoOccurrenceByPayeeName
        };
    }
}
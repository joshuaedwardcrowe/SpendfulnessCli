using Ynab;
using Ynab.Collections;
using Ynab.Responses.Transactions;
using Ynab.Sanitisers;
using YnabProgressConsole.Compilation.Extensions;

namespace YnabProgressConsole.Compilation.Tests.Extensions;

[TestFixture]
public class TransactionByMemoOccurrenceByPayeeNameExtensionsTests
{
    [Test]
    public void GivenDeeplyGroupedTransactions_WhenAggregate_ReturnsTransactionMemoOccurrenceAggregate()
    {
        var response = GetResponse();
        
        var transactions = GetCollection(response);
        
        var aggregate = transactions
            .Aggregate()
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
            Amount = 2500
        };

    private static List<TransactionsByMemoOccurrenceByPayeeName> GetCollection(TransactionResponse transactionResponse)
    {
        var transaction = new Transaction(transactionResponse);
        
        var transactionsByMemoOccurrences = new List<TransactionsByMemoOccurrence>
        {
            new()
            {
                Memo = transaction.Memo,
                MemoOccurence = 1,
                Transactions = [transaction]
            },
        };
        
        var transactionsByMemoOccurrenceByPayeeName = new TransactionsByMemoOccurrenceByPayeeName
        {
            PayeeName = transaction.PayeeName,
            TransactionsByMemoOccurences = transactionsByMemoOccurrences
        };
        
        return new List<TransactionsByMemoOccurrenceByPayeeName>
        {
            transactionsByMemoOccurrenceByPayeeName
        };
    }
}
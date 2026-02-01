using Microsoft.Azure.Cosmos;
using YnabSharp;

namespace Spendfulness.Database.Cosmos.Transactions;

public static class SplitTransactionsExtensions
{
    public static TransactionEntity ToTransactionEntity(this SplitTransactions ynabSplit)
    {
        return new TransactionEntity
        {
            Id = ynabSplit.Id,
            PayeeName = ynabSplit.PayeeName,
            CategoryName = ynabSplit.CategoryName,
            Memo = ynabSplit.Memo,
            Amount = ynabSplit.Amount,
        };
    }
    
    public static (string Id, PartitionKey PartitionKey) GetCosmosKeys(this SplitTransactions ynabSplit) 
        => (ynabSplit.Id, new PartitionKey(ynabSplit.Id));
}
using KitCli.Abstractions.Io;
using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Spendfulness.Database.Cosmos;
using Spendfulness.Database.Cosmos.Transactions;
using Spendfulness.Database.Sqlite;
using YnabSharp;

namespace SpendfulnessCli.Commands.Chat.Chat.PreloadDatabase;

public class ChatPreloadDatabaseCliCommandHandler : CliCommandHandler, ICliCommandHandler<ChatPreloadDatabaseCliCommand>
{
    private readonly CosmosSettings _cosmosSettings;
    private readonly CosmosClient _cosmosClient;
    private readonly SpendfulnessBudgetClient _spendfulnessBudgetClient;
    private readonly ICliIo _io;
    
    public ChatPreloadDatabaseCliCommandHandler(IOptions<CosmosSettings> cosmosSettings, CosmosClient cosmosClient, SpendfulnessBudgetClient spendfulnessBudgetClient, ICliIo io)
    {
        _cosmosSettings = cosmosSettings.Value;
        _cosmosClient = cosmosClient;
        _spendfulnessBudgetClient = spendfulnessBudgetClient;
        _io = io;
    }

    public async Task<CliCommandOutcome[]> Handle(ChatPreloadDatabaseCliCommand command, CancellationToken cancellationToken)
    {
        var transactionContainer = GetCosmosTransactionContainer();
        
        var ynabTransactions = await GetYnabTransactions();
        
        var missingYnabTransactions = await GetMissingTransactions(transactionContainer, ynabTransactions, cancellationToken);
        
        var maxItemsPerSecond = GetMaxItemsPerSecond();
        
        for (var nextBatchStartingPoint = 0; nextBatchStartingPoint < missingYnabTransactions.Count; nextBatchStartingPoint += maxItemsPerSecond)
        {
            var initialItemNumber = nextBatchStartingPoint;
            
            var createItemTasks = missingYnabTransactions
                .Skip(nextBatchStartingPoint)
                .Take(maxItemsPerSecond)
                .Select(t => t.ToTransactionEntity())
                .Select((transaction, currentIndex) =>
                {
                    var itemNumber = initialItemNumber + currentIndex + 1;
                    var totalItems = missingYnabTransactions.Count;

                    return CreateItem(
                        totalItems,
                        itemNumber,
                        transactionContainer,
                        transaction,
                        cancellationToken);
                });
            
            await Task.WhenAll(createItemTasks);

            // RU limit is 1000 per second
            await Task.Delay(1000, cancellationToken);
        }
        
        return OutcomeAs($"{missingYnabTransactions.Count} Transactions Preloaded Into Cosmos");
    }
    
    private int GetMaxItemsPerSecond() => _cosmosSettings.PerSecondRuLimit / _cosmosSettings.PerTransactionCreateRuCost;
    
    private Container GetCosmosTransactionContainer()
    {
        var chatDb = _cosmosClient.GetDatabase("chatdb");
        return chatDb.GetContainer("transactions");
    }

    private async Task<List<SplitTransactions>> GetYnabTransactions()
    {
        var budget = await _spendfulnessBudgetClient.GetDefaultBudget();
        
        var allTransactions =  await budget.GetTransactions();
        
        var allTransactionList = allTransactions.ToList();

        var nonSplitTransactions = allTransactionList
            .Where(transaction => !transaction.SplitTransactions.Any())
            .ToList();

        var splitTransactions = allTransactionList
            .Where(transaction => transaction.SplitTransactions.Any())
            .SelectMany(transaction => transaction.SplitTransactions);

        return nonSplitTransactions
            .Concat(splitTransactions)
            .ToList();
    }

    private async Task<List<SplitTransactions>> GetMissingTransactions(Container transactionContainer, List<SplitTransactions> ynabTransactions, CancellationToken cancellationToken)
    {
        var ynabTransactionCosmosKeys = ynabTransactions
            .Select(ynabTransaction => ynabTransaction.GetCosmosKeys())
            .ToList();
        
        var feedResponse = await transactionContainer.ReadManyItemsAsync<TransactionEntity>(
            ynabTransactionCosmosKeys, null, cancellationToken);

        var existingItemIds = feedResponse.Select(cosmosTransaction => cosmosTransaction.Id).ToList();
        
        var missingTransactionIds =  ynabTransactionCosmosKeys
            .Where(identifier => !existingItemIds.Contains(identifier.Id))
            .Select(identifier => identifier.Id)
            .ToList();
        
        return ynabTransactions
            .Where(transaction => missingTransactionIds.Contains(transaction.Id))
            .ToList();
    }

    private async Task CreateItem(int totalItems, int itemNumber, Container container, TransactionEntity transactionEntity, CancellationToken cancellationToken)
    {
        _io.Say($"On Item #{itemNumber} / {totalItems} (PartitionKey: {transactionEntity.Id})");
        
        await container.CreateItemAsync(
            transactionEntity,
            new PartitionKey(transactionEntity.Id),
            null,
            cancellationToken);
    }
}
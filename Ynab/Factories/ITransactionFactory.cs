using Ynab.Responses.Transactions;

namespace Ynab.Factories;

public interface ITransactionFactory
{
    bool CanWorkWith(TransactionResponse transactionResponse);
    
    Transaction Create(TransactionResponse transactionResponse);
}
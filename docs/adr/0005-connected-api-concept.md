# Connected API Concept

## Premise
When working with YNAB API objects, developers often need to perform related operations that require accessing different API endpoints. For example, to delete a transaction, you need to call the transaction client with the transaction's ID. To get all transactions for an account, you need to call the transaction client and filter by account ID.

This creates a disconnected experience where the data objects are passive containers that require external clients to perform operations, similar to working with raw DTOs rather than rich domain objects.

## Problem
The current approach requires developers to:

1. **Manage multiple client instances** - Need to inject or instantiate various clients (AccountClient, TransactionClient, etc.) to perform operations
2. **Manually coordinate between objects and clients** - Must remember which client handles which operations for each type of object
3. **Write repetitive filtering logic** - Often need to filter collections (e.g., transactions for a specific account) at the application level
4. **Lack object-oriented operations** - Cannot call methods directly on the objects themselves (e.g., `transaction.Delete()` or `account.GetTransactions()`)

This leads to verbose, error-prone code where the relationship between data and operations is not immediately clear.

## Solution
The Connected API concept introduces "Connected" versions of the normal DTOs that encapsulate their respective API clients. This enables an ORM-like experience where objects can perform operations on themselves through the YNAB API.

### Implementation Approach
1. **Connected Classes**: Create classes like `ConnectedBudget`, `ConnectedAccount`, `ConnectedTransaction` that inherit from their respective base classes
2. **Client Encapsulation**: Each Connected class contains the necessary API clients as private fields
3. **Method Delegation**: Connected classes expose methods that delegate to the appropriate client methods
4. **Object Traversal**: Connected objects can return other Connected objects, maintaining the connected experience throughout the object graph

### Example Usage
```csharp
// Instead of this disconnected approach:
var transactionClient = new TransactionClient(builder, budgetPath);
var transactions = await transactionClient.GetAll();
var accountTransactions = transactions.Where(t => t.AccountId == accountId);

// Use this connected approach:
var connectedAccount = await budget.GetAccount(accountId);
var transactions = await connectedAccount.GetTransactions();

// Or even:
await connectedTransaction.Delete(); // Future enhancement
```

### Current Implementation
- **ConnectedBudget**: Encapsulates AccountClient, CategoryClient, TransactionClient, and ScheduledTransactionClient
  - Provides methods like `GetAccounts()`, `GetAccount(id)`, `CreateAccount()`, `MoveAccountTransactions()`
- **ConnectedAccount**: Encapsulates TransactionClient and ScheduledTransactionClient  
  - Provides methods like `GetTransactions()`, `GetScheduledTransactions()`

### Factory Pattern Integration
The `AccountClient.Get()` and `AccountClient.Create()` methods return `ConnectedAccount` instances rather than basic `Account` objects, ensuring the connected experience is maintained throughout the API usage.

## Constraints

### Performance Considerations
- Connected objects hold references to multiple clients, which may increase memory usage
- Each Connected object creates new client instances, which could lead to resource overhead
- Consider implementing client sharing or pooling for better resource management

### API Surface Complexity
- Connected classes expose both inherited data properties and new operational methods
- Need to maintain clear separation between data access and API operations
- Must ensure Connected classes don't become too large or violate single responsibility principle

### Dependency Management
- Connected objects require proper client configuration and authentication
- Client lifecycle management becomes more complex when embedded in data objects
- Error handling needs to be consistent across all Connected operations

## Questions & Answers

### Why not add these methods to the base classes directly?
The base classes (Budget, Account, Transaction) are designed as pure data transfer objects that mirror the YNAB API responses. Adding API client dependencies would violate their single responsibility and make them harder to test and serialize.

### How does this differ from the Repository pattern?
The Repository pattern typically provides a single interface for data access operations. The Connected concept embeds operational capabilities directly into the data objects themselves, creating a more object-oriented experience similar to Active Record pattern.

### What about testing Connected objects?
Connected objects can be tested by mocking the underlying client dependencies. Consider providing factory methods or dependency injection patterns to make testing easier.

### Should all YNAB objects have Connected versions?
Not necessarily. Connected versions should be created for objects that benefit from having operational methods. Read-only or simple value objects may not need Connected versions.

### How does this handle client authentication and configuration?
Connected objects receive pre-configured clients through their constructors. The client configuration (authentication, base URLs, etc.) is handled at the client level before the Connected object is created.

### What about caching and performance optimization?
Connected objects could implement caching strategies for frequently accessed data. However, care must be taken to ensure cache invalidation when underlying data changes through API operations.
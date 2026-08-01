# Aggregation Concept

## Premise

When working with YNAB financial data, developers frequently need to transform raw data (transactions, accounts, category groups) into meaningful insights for reporting and analysis. This often involves complex operations like filtering, grouping, calculating averages, totals, and other statistical operations across different time periods or categories.

The challenge is that these operations tend to be repetitive across different commands and reports, leading to code duplication and inconsistent implementation of similar business logic.

## Problem

Without a structured approach to data aggregation, the codebase suffers from:

1. **Code Duplication** - Similar filtering and aggregation logic scattered across different command handlers
2. **Inconsistent Operations** - Different implementations of similar business operations (e.g., calculating averages, filtering by account types)
3. **Complex Command Handlers** - Command handlers becoming bloated with data transformation logic that should be reusable
4. **Difficult Testing** - Data transformation logic mixed with command handling makes unit testing more complex
5. **Poor Separation of Concerns** - Business logic for data aggregation mixed with presentation logic

## Solution

The Aggregation concept introduces a structured approach to data transformation through two main abstractions:

### 1. Aggregator<TAggregation>
The base aggregator class provides a fluent interface for applying operations to YNAB data before generating the final aggregate result.

**Key Features:**
- **Data Sources**: Handles Accounts, CategoryGroups, Transactions, and Commitments
- **Pre-Aggregation Operations**: Allows filtering and transformation via `BeforeAggregation()` methods
- **Generic Type Safety**: Strongly typed aggregation results through `TAggregation`
- **Fluent Interface**: Chainable method calls for readable operation composition

**Example Usage:**
```csharp
var aggregator = new CategoryDeductedAmountAggregator(accounts, categoryGroups)
    .BeforeAggregation(a => a.FilterToTypes(AccountType.Checking, AccountType.Savings))
    .BeforeAggregation(FilterToCriticalCategoryGroups);

var result = aggregator.Aggregate();
```

### 2. ListAggregator<TAggregate>
Specialized aggregator for operations that produce collections of aggregate objects, extending the base aggregator with post-aggregation operations.

**Additional Features:**
- **Post-Aggregation Operations**: Allows further transformation via `AfterAggregation()` methods
- **Collection Operations**: Specialized for operations on lists of aggregated data

### 3. Aggregate Records
Strongly-typed data structures representing the final aggregated results:

```csharp
public record CategoryAggregate(Guid CategoryId, string CategoryName);
public record TransactionYearAverageAggregate(string Year, decimal AverageAmount, int PercentageChange);
```

### Implementation Architecture

1. **Aggregator Base Class**: Provides common functionality for data source management and pre-aggregation operations
2. **Specialized Aggregators**: Concrete implementations for specific business operations (e.g., `CategoryDeductedAmountAggregator`, `TransactionAverageAcrossYearAggregator`)
3. **Aggregate Models**: Simple record types representing the final aggregated data
4. **Integration with ViewModels**: Aggregators integrate seamlessly with ViewModelBuilders for consistent data presentation

### Current Implementations

**Amount Aggregators:**
- `CategoryDeductedAmountAggregator`: Calculates amounts after category deductions

**List Aggregators:**
- `CategoryAggregator`: Aggregates category information
- `TransactionAggregator`: Aggregates transaction data
- `TransactionAverageAcrossYearAggregator`: Calculates averages across years
- `TransactionMonthTotalAggregator`: Monthly transaction totals

## Constraints

### Data Dependency Management
- Aggregators require proper initialization with appropriate data sources (accounts, categories, transactions)
- Some aggregators may require specific combinations of data sources to function correctly
- Data consistency between different sources must be maintained

### Performance Considerations
- Large datasets may require optimization of aggregation operations
- Memory usage can increase with complex aggregation chains
- Consider lazy evaluation for expensive operations

### Type Safety
- Generic type parameters must be correctly specified for aggregation results
- Aggregate models should remain immutable to prevent data corruption
- Null handling must be consistent across all aggregator implementations

### Operation Order
- Pre-aggregation operations are applied in the order they are chained
- Post-aggregation operations (for ListAggregators) are applied after the core aggregation
- Order dependencies should be documented for complex aggregation chains

## Questions & Answers

### Why separate BeforeAggregation and AfterAggregation operations?

BeforeAggregation operations work on the raw data sources (accounts, transactions, etc.) before the core aggregation logic runs. AfterAggregation operations work on the already-aggregated results. This separation allows for more precise control over when transformations are applied and ensures that the core aggregation logic receives properly filtered/prepared data.

### How does this integrate with the ViewModelBuilder pattern?

Aggregators are designed to work seamlessly with ViewModelBuilders through the `WithAggregator()` method. The ViewModelBuilder calls `aggregator.Aggregate()` to get the processed data, then transforms it into presentation-ready format. This maintains clean separation between data aggregation and presentation logic.

### Should all data transformations use aggregators?

Aggregators are best suited for reusable, complex data transformations that involve multiple data sources or statistical operations. Simple transformations that are used only once might be better handled directly in the command handler. The key is reusability and complexity.

### How do you handle aggregators that need different data source combinations?

The base Aggregator class provides multiple constructors to handle different combinations of data sources:
- Default constructor (empty data sources)
- Single data source constructors (e.g., transactions only)
- Multiple data source constructors (e.g., accounts + category groups)

This flexibility allows each aggregator to request only the data it actually needs.

### What about error handling in aggregation operations?

Currently, aggregation operations rely on the underlying data integrity and standard .NET exception handling. For production use, consider adding validation for required data sources and graceful handling of edge cases like empty datasets or invalid date ranges.

### How do you test complex aggregation chains?

Aggregators can be unit tested by providing mock data sources and verifying the aggregated results. The fluent interface allows testing both individual operations and complete aggregation chains. Consider creating test data builders for consistent test scenarios across different aggregator implementations.
# Compilation

Deciding how a data set is presented.

Four types sit between YNAB's domain objects and a console table, in two
projects: `Spendfulness.Aggregation` reduces, `SpendfulnessCli.CliTables`
renders. The split matters — aggregation knows nothing about tables, so the
same aggregate can feed a table, a CSV export, or a chat response.

```
YnabSharp domain types
        │  Aggregator      (Spendfulness.Aggregation/Aggregator)
        ▼
    Aggregate             (Spendfulness.Aggregation/Aggregates)
        │  ViewModelBuilder (SpendfulnessCli.CliTables/ViewModelBuilders)
        ▼
     ViewModel            (SpendfulnessCli.CliTables/ViewModels)
```

## Aggregate

A record describing one reduced row, and nothing else — no formatting, no
column names:

```csharp
public record CategoryAggregate(Guid CategoryId, string CategoryName);
```

They live in `Spendfulness.Aggregation/Aggregates`, one per shape of answer
(`TransactionYearAverageAggregate`, `TransactionMonthTotalAggregate`, …).

## Aggregator

Turns YnabSharp domain objects into aggregates. List aggregators derive
from `YnabListAggregator<TAggregate>` and implement `GenerateAggregate()`:

```csharp
public class CategoryYnabListAggregator(IEnumerable<CategoryGroup> categoryGroups)
    : YnabListAggregator<CategoryAggregate>(categoryGroups)
{
    protected override IEnumerable<CategoryAggregate> GenerateAggregate()
        => CategoryGroups
            .SelectMany(categoryGroup => categoryGroup.Categories)
            .Select(category => new CategoryAggregate(category.Id, category.Name));
}
```

Filtering is composed by the caller rather than baked in, via
`BeforeAggregation` hooks that run against the source before reduction:

```csharp
new TransactionAverageAcrossYearYnabListAggregator(transactions)
    .BeforeAggregation(y => y.FilterToSpending())
    .BeforeAggregation(y => y.FilterToOutflow());
```

`Aggregator/ListAggregators` produces sequences; `Aggregator/AmountAggregators`
reduces to single figures.

## ViewModel

Not a data carrier — it owns the *column vocabulary* for a table, as
constants plus their order:

```csharp
public class CategoryViewModel
{
    public const string CategoryName = "Category Name";
    public const string CategoryId = "Category Id";

    public static List<string> GetColumnNames() => [CategoryName, CategoryId];
}
```

Keeping headings as constants means a column can be referenced by name from
a filter or test without a string literal drifting out of sync with what is
printed.

## ViewModelBuilder

Derives from `CliTableBuilder<TAggregate>` and maps aggregates onto that
vocabulary — one method for the headings, one for the rows:

```csharp
public class CategoryCliTableBuilder : CliTableBuilder<CategoryAggregate>
{
    protected override List<string> BuildColumnNames(IEnumerable<CategoryAggregate> evaluation)
        => CategoryViewModel.GetColumnNames();

    protected override List<List<object>> BuildRows(IEnumerable<CategoryAggregate> aggregates)
        => aggregates
            .Select(a => new List<object> { a.CategoryName, a.CategoryId })
            .ToList();
}
```

Handlers use it fluently, which is the point at which the two projects meet:

```csharp
var viewModel = new CategoryCliTableBuilder().WithAggregator(aggregator).Build();
```

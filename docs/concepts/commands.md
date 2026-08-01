# Commands

How an instruction becomes a rendered table.

The command base types come from [KitCli](https://github.com/KitCli/KitCli)
(`KitCli.Commands.Abstractions`), not this repo. What lives here is one
triple per command, plus the registration that makes it discoverable.

## The triple

Every command is three small types in one folder, named after the command:

```
SpendfulnessCli.Commands.Reporting/AverageYearlySpending/
├── AverageYearlySpendingCliCommand.cs         the request
├── AverageYearlySpendingCliCommandFactory.cs  instruction → request
└── AverageYearlySpendingCliCommandHandler.cs  request → outcomes
```

### Command

A record deriving from `CliCommand`. It carries the arguments the handler
needs and nothing else — a command taking no arguments is legitimately
empty:

```csharp
public record AverageYearlySpendingCliCommand : CliCommand;
```

### Command Factory

Implements `ICliCommandFactory<TCommand>`. Turns a parsed `CliInstruction`
and the accumulated `CliCommandArtefact`s into the command record. This is
where raw argument text becomes typed values, so the handler never parses
anything:

```csharp
public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    => new AverageYearlySpendingCliCommand();
```

### Command Handler

Derives from `CliCommandHandler` and implements
`ICliCommandHandler<TCommand>`. It orchestrates rather than calculates, and
the shape is consistent: get a budget, aggregate, build a view model,
return outcomes.

```csharp
public async Task<CliCommandOutcome[]> Handle(
    AverageYearlySpendingCliCommand request, CancellationToken cancellationToken)
{
    var budget = await spendfulnessBudgetClient.GetDefaultBudget();
    var transactions = await budget.GetTransactions();

    var aggregator = new TransactionAverageAcrossYearYnabListAggregator(transactions)
        .BeforeAggregation(y => y.FilterToSpending())
        .BeforeAggregation(y => y.FilterToOutflow());

    var viewModel = new TransactionYearAverageCliTableBuilder()
        .WithAggregator(aggregator)
        .Build();

    return OutcomeAs(viewModel);
}
```

`OutcomeAs(...)` wraps the result as `CliCommandOutcome[]`. A handler
returns outcomes instead of writing to the console itself, so rendering
stays the framework's job — [compilation](compilation.md) covers what it
hands over.

The budget always arrives through `SpendfulnessBudgetClient`, never a raw
client; see [YNAB client layers](ynab-client-layers.md).

## Grouping

Commands are split into projects by area, matching the `scope` used in PR
titles: `Reporting`, `Organisation`, `Personalisation`, `Export.Csv`,
`Chat`, and `Reusable` for commands shared across areas. Each project
registers its own triples through its `ServiceCollectionExtensions`, so
adding a command means touching one project rather than a central list.

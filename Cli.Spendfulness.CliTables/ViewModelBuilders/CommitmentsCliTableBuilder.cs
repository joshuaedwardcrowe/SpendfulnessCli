using Cli.Spendfulness.CliTables.Extensions;
using Ynab.Sanitisers;
using Cli.Spendfulness.Aggregation.Calculators;
using Cli.Spendfulness.CliTables.Formatters;
using Cli.Spendfulness.Database.Commitments;

namespace Cli.Spendfulness.CliTables.ViewModelBuilders;

public class CommitmentsCliTableBuilder : CliTableBuilder<IEnumerable<Commitment>>
{
    protected override List<string> BuildColumnNames(IEnumerable<Commitment> evaluation)
        => [
            nameof(Commitment.Name),
            nameof(Commitment.Started),
            nameof(Commitment.RequiredBy),
            nameof(Commitment.Funded),
            nameof(Commitment.Needed),
            "% Met"
        ];

    protected override List<List<object>> BuildRows(IEnumerable<Commitment> aggregates)
        => aggregates
            .OrderBySortOrder(CliTableSortOrder, c => c.RequiredBy)
            .Select(BuildRow)
            .ToList();

    private List<object> BuildRow(Commitment commitment)
    {
        var displayableStarted = commitment.Started.HasValue
            ? IdentifierSanitiser.SanitiseForMonth(commitment.Started.Value)
            : string.Empty;

        var displayableRequiredBy = commitment.RequiredBy.HasValue
            ? IdentifierSanitiser.SanitiseForMonth(commitment.RequiredBy.Value)
            : string.Empty;

        var displayableFunded = commitment.Funded.HasValue
            ? CurrencyDisplayFormatter.Format(commitment.Funded.Value)
            : string.Empty;

        var displayableNeeded = commitment.Needed.HasValue
            ? CurrencyDisplayFormatter.Format(commitment.Needed.Value)
            : string.Empty;

        var percentageMet = commitment is { Funded: not null, Target: not null }
            ? PercentageCalculator.Calculate(commitment.Funded.Value, commitment.Target.Value)
            : 0;
        
        var displayablePercentageMet = PercentageDisplayFormatter.Format(percentageMet);

        return
        [
            commitment.Name,
            displayableStarted,
            displayableRequiredBy,
            displayableFunded,
            displayableNeeded,
            displayablePercentageMet
        ];
    }
}
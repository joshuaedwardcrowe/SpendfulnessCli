namespace Cli.Spendfulness.Commands.Aggregate;

public record CommandHelpAggregate(
    string Call,
    CommandActionType Type,
    string Summary);
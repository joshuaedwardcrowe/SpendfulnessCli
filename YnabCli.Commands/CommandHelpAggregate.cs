namespace YnabCli.Commands;

public record CommandHelpAggregate(
    string Call,
    CommandActionType Type,
    string Summary);
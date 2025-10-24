namespace Ynab.Exceptions;

// TODO: Abstract into a CliExceptionCode, CliOutcomeExeptionCode etc.
public enum YnabExceptionCode
{
    ApiResponseIsEmpty,
    ApiResponseIsError,
    CloseAccountCannotBeMovedOnBudget,
    OnBudgetAccountCannotBeMovedOnBudget,
    NoInstruction,
    NoCommandGenerator,
    UnknownCliCommandOutcome,
}
namespace YnabCli.Commands.Ynab.FlagChanges;

public class FlagChangesCommand : ICommand
{
    public const string CommandName = "flag-changes";
    public const string ShorthandCommandName = "fc";
    
    public static class ArgumentNames
    {
        public const string From = "from";
        public const string To = "to";
    }
    
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
}
namespace YnabCli.Commands.Database.Commitments;

public class CommitmentCommand : ICommand
{
    public const string CommandName = "commitment";
    public const string ShorthandCommandName = "c";

    public static class SubCommandNames
    {
        public const string Find = "find";
    }
}
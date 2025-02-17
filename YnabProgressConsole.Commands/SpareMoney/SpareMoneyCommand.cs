namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommand : ICommand
{
    public const string CommandName = "spare-money";
    public const string ShorthandCommandName = "sm";

    public static class ArgumentNames
    {
        public const string MinusSavings = "minus-savings";
    }
    
    public bool? MinusSavings { get; set; }
}